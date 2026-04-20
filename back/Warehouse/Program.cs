using System.Security.Cryptography.X509Certificates;
using WarehouseService;

namespace Warehouse;

public class Program
{
    public class program
    {
        
        public static async Task Main(string[] args)
        {

            Warehouse wh = new Warehouse("1", "wh1", "http://localhost:8081/Service.asmx");

            var s = wh.GetConnection().GetService().GetInventoryAsync();
            Console.WriteLine(s);
        }
    }
}