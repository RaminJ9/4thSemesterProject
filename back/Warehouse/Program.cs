using System.Security.Cryptography.X509Certificates;
using Common.Models;
using WarehouseService;

namespace Warehouse;

public class Program
{
    public class program
    {
        
        public static async Task Main(string[] args)
        {

            Warehouse wh = new Warehouse("1", "wh1", "http://localhost:8081/Service.asmx");
            Tray tray = new Tray(0, "Parts");

            var s = wh.Provide(tray);
            Console.WriteLine(s);
        }
    }
}