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
            var json = await this.GetWarehouseInventory();
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception($"Unable to retrieve Warehouse: {Guid} inventory");
            }
            var doc = JsonDocument.Parse(json);

            
            // Get the inventory object (first element of the array)
            var inventory = doc.RootElement.GetProperty("Inventory");
            
            // Iterate over tray slots
            try
            {
                Console.WriteLine(inventory.ToString());
                if (inventory.GetArrayLength()==0)
                {
                    throw new Exception($"Warehouse: {Guid} Inventory is empty"); // If warehouse is empty.
                }
                foreach (var item in inventory.EnumerateArray())
                {
                    Console.WriteLine(item);
                    if (item.GetProperty("Content").GetString().Contains(tray.Name)) // this needs to be changed to parts, after testing.
                    {
                        Tray returnTray = new Tray(item.GetProperty("Id").GetInt32(), tray.Name);
                        await this.GetConnection().GetService().PickItemAsync(returnTray.Id);
                        return returnTray;
                    }
                    // When providing with the given method PickItemAsync, it only provides the parts, but since we disgussed it should provide a tray, 
                    // we will provide it as a tray but the method doesnt do it like that.
                    // So when we recieve a tray it will kinda merge, into one tray since there already was one. I think, not sure yet.
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
             
            throw new Exception($"No parts in Warehouse : {Guid}"); // if no parts in Warehare.
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
