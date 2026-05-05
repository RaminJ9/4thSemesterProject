using Common.Models;

namespace AGV
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AGV agv = new AGV(
                guid: "test-guid",
                name: "TestAGV",
                connectionString: "http://localhost:8082/v1/status/"
            );

            //Test Get tatus
            Console.WriteLine("Testing GetStatus");
            var status = await agv.GetStatus();
            Console.WriteLine($"Status: {status}");

            //Test get battery
            Console.WriteLine("\nTesting GetBattery");
            var battery = await agv.GetBattery();
            Console.WriteLine($"Battery: {battery}%");

            //Test Go to charger
            Console.WriteLine("\nTesting GoToCharger");
            await agv.GoToCharger();
            Console.WriteLine("AGV sent to charger");

            //Test receive with a part tray (name contains "item")
            Console.WriteLine("\nTesting Receive (Part)");
            var partTray = new Tray(1, "Item 1");
            await agv.Receive(partTray);
            Console.WriteLine("Receive part done");

            //Test receive with a drone tray
            Console.WriteLine("\nTesting Receive (Drone)");
            var droneTray = new Tray(2, "Assembly 1");
            await agv.Receive(droneTray);
            Console.WriteLine("Receive drone done");

            //Test provide with a part tray
            Console.WriteLine("\nTesting Provide (Part)");
            var partTray2 = new Tray(3, "Item 2");
            await agv.Provide(partTray2);
            Console.WriteLine("Provide part done");

            //Test provide with a drone tray
            Console.WriteLine("\nTesting Provide (Drone)");
            var droneTray2 = new Tray(4, "Assembly 2");
            await agv.Provide(droneTray2);
            Console.WriteLine("Provide drone done");

        }
    }
}