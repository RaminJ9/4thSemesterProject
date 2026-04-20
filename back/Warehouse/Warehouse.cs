using Common.Contracts;
using Common.Models;
using System.Composition;
using WarehouseService;

namespace Warehouse
{
    [Export(typeof(MachineComponentBase))]
    public class Warehouse : MachineComponentBase
    {
        public Warehouse(string guid, string name, string connectionString) : base(guid, name, connectionString)
        {
        }

        public override Task<Tray?> Provide(Tray tray)
        {
            throw new NotImplementedException();
        }

        public override Task<Tray?> Receive(Tray tray)
        {
            throw new NotImplementedException();
        }
        
        
    }
    
    
}
