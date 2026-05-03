using Common.Contracts;
using Common.Models;
using System.Composition;
using System.Net.Http.Json;
using System.Text.Json;

namespace AGV
{
    [Export(typeof(MachineComponentBase))]
    public class AGV : MachineComponentBase
    {

        //Item = Parts & Asembly = drone
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public AGV(string guid, string name, string connectionString) : base(guid, name, connectionString)
        {
            _httpClient = new HttpClient();
            _url = connectionString;
        }

        //Provide: AGV gives away to the next machine 
        public override async Task<Tray?> Provide(Tray tray)
        {
            if (tray == null)
                throw new Exception("Tray is null");

            //Parts go to assembly, drones go to warehouse

            if (IsPart(tray)) //If tray is parts then move to assembly
            {
                await RunProgram("MoveToAssemblyOperation");
                await RunProgram("PutAssemblyOperation");
            }
            else //If tray is drones then move to warehouse
            {
                await RunProgram("MoveToStorageOperation");
                await RunProgram("PutWarehouseOperation");
            }

            return null;
        }

        //Receive: AGV picks up from another machine
        public override async Task<Tray?> Receive(Tray tray)
        {
            if (tray == null)
                throw new Exception("Tray is null");

            //Parts come from warehouse, drones come from assembly

            if (IsPart(tray)) //If tray is parts then move to warehouse
            {
                await RunProgram("MoveToStorageOperation");
                await RunProgram("PickWarehouseOperation");
            }
            else //If tray is drones then move to assembly
            {
                await RunProgram("MoveToAssemblyOperation");
                await RunProgram("PickAssemblyOperation");
            }

            return tray;
        }

        public async Task<string> GetStatus()
        {
            var response = await _httpClient.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<int?> GetBattery()
        {
            var json = await GetStatus();
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("battery", out var battery))
                return battery.GetInt32();

            if (doc.RootElement.TryGetProperty("Battery", out var batteryUpper))
                return batteryUpper.GetInt32();

            return null;
        }

        public async Task GoToCharger()
        {
            await LoadProgram("MoveToChargerOperation");
            await ExecuteLoadedProgram();
        }

        //Method to load a program and run it in one instead of running both seperate (Read below to understand)
        private async Task RunProgram(string programName)
        {
            await EnsureBatteryForOperation();
            await WaitUntilAgvReady();
            await LoadProgram(programName);
            await ExecuteLoadedProgram();
            await WaitUntilAgvReady();
        }

        //this makes sure that AGV goes charging if battery is too low before doing the next operation
        private async Task EnsureBatteryForOperation()
        {
            int? battery = await GetBattery();

            if (battery == null)
                throw new Exception("Could not read AGV battery");

            if (battery > 10)
                return;

            await GoToCharger();

            while (true)
            {
                await Task.Delay(2000);

                battery = await GetBattery();

                if (battery == null)
                    throw new Exception("Could not read AGV battery while charging");

                if (battery >= 80)
                    break;
            }

            await Task.Delay(2000);
        }

        //AGV has to use these two methods to execute. Read technical documentation to understand
        //Step 1 = Load what you want it to do (LoadProgram) It just tells the AGV: “Next time you run, do this program”
        //Step 2 = Tell it to start doing it (ExecuteLoadedProgram) This tells “Start executing the program you just loaded”

        private async Task LoadProgram(string programName)
        {
            var program = new Dictionary<string, object>
            {
                ["Program name"] = programName,
                ["State"] = 1
            };

            var response = await _httpClient.PutAsJsonAsync(_url, program);
            var responseBody = await response.Content.ReadAsStringAsync();

            //for testing
            Console.WriteLine($"LoadProgram request: {programName}");
            Console.WriteLine($"Status code: {(int)response.StatusCode}");
            Console.WriteLine($"Response body: {responseBody}");

            response.EnsureSuccessStatusCode();
        }

        //For testing in Program.cs
        public async Task TestProgram(string programName)
        {
            await RunProgram(programName);
        }

        private async Task ExecuteLoadedProgram()
        {
            var payload = new Dictionary<string, object>
            {
                ["State"] = 2
            };

            var response = await _httpClient.PutAsJsonAsync(_url, payload);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Execute status code: {(int)response.StatusCode}");
            Console.WriteLine($"Execute response body: {responseBody}");

            response.EnsureSuccessStatusCode();
        }

        //check if its a drone or part
        private bool IsPart(Tray tray)
        {
            var name = tray.Name?.ToLower() ?? "";

            return name.Contains("item");
        }

        //Got the problem "Already executing a task", so this methods makes sure the AGV is ready for a new task before getting a new one.
        //It check if the AGV state=2 (state 2 means "Executing") if not then it returns. 
        private async Task WaitUntilAgvReady()
        {
            while (true)
            {
                var json = await GetStatus();
                using var doc = JsonDocument.Parse(json);

                int state = doc.RootElement.GetProperty("state").GetInt32();

                if (state != 2)
                    return;

                Console.WriteLine("AGV is executing a task WAIT");
                await Task.Delay(500);
            }
        }
    }
}