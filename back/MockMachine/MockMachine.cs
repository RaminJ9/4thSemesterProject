using Common.Contracts;
using Common.Models;
using System.Composition;

namespace MockMachine
{
    [Export(typeof(MachineComponentBase))]
    public class MockMachine : MachineComponentBase
    {
        private Tray? tray;
        public MockMachine(string guid, string name, string connectionString) : base(guid, name, connectionString) { }
        public async override Task<Tray?> Provide(Tray tray)
        {
            if (tray == null) return null;
            await Task.Delay(1000);
            return tray;
        }

        public async override Task<Tray?> Receive(Tray tray)
        {
            await Task.Delay(500);
            this.tray = tray;
            return this.tray;
        }
    }
}
