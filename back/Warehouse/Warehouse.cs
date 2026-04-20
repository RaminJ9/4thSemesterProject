using Common.Contracts;
using Common.Models;
using System.Composition;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WarehouseService;

namespace Warehouse
{
    [Export(typeof(MachineComponentBase))]
    public class Warehouse : MachineComponentBase
    {
        private Connection connection;
        public Warehouse(string guid, string name, string connectionString) : base(guid, name, connectionString)
        {
            try
            {
                connection = new Connection(connectionString);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            //Fill warehouse with parts
        }

        public override async Task<Tray?> Provide(Tray tray) //Provides a tray of parts
        {
            var jsonInv = await this.GetWarehouseInventory();
            var trays = JsonSerializer.Deserialize<dynamic>(jsonInv);
            
            foreach(var t in trays)
            {
                Console.WriteLine(t);
            }
            return null;
            //GetConnection().GetService().PickItemAsync(tray_id);
        }

        public override Task<Tray?> Receive(Tray tray) //Ignore ID, use name to figure out if providing Drone or Parts
        {
            throw new NotImplementedException();
        }
        
        public Connection GetConnection()
        {
            return connection;
        }
        
        public async Task<string> GetWarehouseInventory()
        {
            var s = await this.GetConnection().GetService().GetInventoryAsync();
            return s.ToString();
        }

    }
    
    
}
