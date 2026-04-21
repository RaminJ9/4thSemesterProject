using Common.Contracts;
using Common.Models;
using System.Composition;
using System.Diagnostics;

namespace MockMachine
{
    [Export(typeof(MachineComponentBase))]
    public class MockMachine : MachineComponentBase
    {
        private Tray? tray;
        private int delay;
        private int? errorAtProvideCycle;
        private int? errorAtReceiveCycle;
        private int provideCycle = 1;
        private int receiveCycle = 1; 

        /// The extra settings for MockMachine (in connectionString) follows this format:
        /// "delay;errorAtProvideCycle;errorAtReceiveCycle"
        /// 
        /// delay = Delay for each operation in ms (default = 750)
        /// errorAtProvideCycle = The cycle, at which there will be an error 
        /// whilst providing (set to -1 for no errors)
        /// errorAtReceiveCycle = The cycle, at which there will be an error 
        /// whilst receiving (set to -1 for no errors)
        /// 
        /// No matter which connection string you use, it will be randomized.
        /// This is done so multiple machine can have the samme extra settings
        /// without causing a "DuplicateMachineException".
        public MockMachine(string guid, string name, string connectionString) : base(guid, name, RandomizeConnectionString(connectionString))
        {
            try
            {
                string[] settings = connectionString.Split(';');
                delay = int.Parse(settings[0]);
                errorAtProvideCycle = int.Parse(settings[1]);
                errorAtReceiveCycle = int.Parse(settings[2]);
            } catch
            {
                delay = 750;
            }
        }

        private static string RandomizeConnectionString(string connectionString) => $"{connectionString};{System.Guid.NewGuid().ToString()}"; 
        public async override Task<Tray?> Provide(Tray tray)
        {
            if (provideCycle == errorAtProvideCycle)
            {
                throw new Exception($"MockMachine; '{Guid}', '{Name}', '{ConnectionString}' experienced error whilst providing:\nOh no! Spooky scary error!!");
            }
            if (tray == null) return null;
            await Task.Delay(delay);
            Debug.WriteLine($"{Name}: Providing");
            provideCycle++;
            return tray;
        }

        public async override Task<Tray?> Receive(Tray tray)
        {
            if (receiveCycle == errorAtReceiveCycle)
            {
                throw new Exception($"MockMachine; '{Guid}', '{Name}', '{ConnectionString}' experienced error whilst receiving:\nOh no! Spooky scary error!!");
            }
            await Task.Delay(delay);
            this.tray = tray;
            Debug.WriteLine($"{Name}: Receiving");
            receiveCycle++;
            return this.tray;
        }
    }
}
