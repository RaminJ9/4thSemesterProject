using Common.Models;

namespace AGV
{
    public class Program
    {
        //**************** TEST ******************
        //Delete this when we integrate with "common"
        public static async Task Main(string[] args)
        {
            AGV agv = new AGV("1", "agv1", "http://localhost:8082/v1/status/");

            Tray partTray = new Tray(1, "Item");
            Tray droneTray = new Tray(2, "Assembly");

            Console.WriteLine("Initial status:");
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nBattery:");
            Console.WriteLine(await agv.GetBattery());

            Console.WriteLine("\nTesting MoveToChargerOperation...");
            await agv.TestProgram("MoveToChargerOperation");
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting MoveToStorageOperation...");
            await agv.TestProgram("MoveToStorageOperation");
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting MoveToAssemblyOperation...");
            await agv.TestProgram("MoveToAssemblyOperation");
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting Receive(part)...");
            await agv.Receive(partTray);
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting Provide(part)...");
            await agv.Provide(partTray);
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting Receive(drone)...");
            await agv.Receive(droneTray);
            Console.WriteLine(await agv.GetStatus());

            Console.WriteLine("\nTesting Provide(drone)...");
            await agv.Provide(droneTray);
            Console.WriteLine(await agv.GetStatus());
        }
    }
}