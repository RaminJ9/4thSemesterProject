using WarehouseService;

namespace Warehouse;

public class Program
{
    public class program
    {
        public static async Task Main(string[] args)
        {

            SOAP conn = new SOAP();

            await conn.RunExample();

            Console.WriteLine("Testing");

        }
    }
}