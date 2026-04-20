using WarehouseService;

namespace Warehouse;

public class Program
{
    public class program
    {
        public static async Task Main(string[] args)
        {

            Connection conn = new Connection();

            await conn.RunExample();

            Console.WriteLine("Testing");

        }
    }
}