using System;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Hardware;

namespace ThreeDimensionPrinter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("3D Printer Motor Test");
            Console.WriteLine("====================");
            Console.WriteLine();

            MotorFactory.VendorType xMotor = MotorFactory.VendorType.VendorA;
            MotorFactory.VendorType yMotor = MotorFactory.VendorType.VendorA;
            MotorFactory.VendorType zMotor = MotorFactory.VendorType.VendorB;
            var printer = new ThreeAxisPrinter(xMotor, yMotor, zMotor);

            try
            {
                Console.WriteLine("Initializing printer...");
                await printer.Initialize();

                Console.WriteLine("\nMoving to position (10, 10, 10)...");
                await printer.Move(10, 10, 10);

                Console.WriteLine("\nMoving to position (20, 20, 20)...");
                await printer.Move(20, 20, 20);

                Console.WriteLine("\nMoving to position (25, 25, 25)...");
                await printer.Move(25, 25, 25);

                Console.WriteLine("\nMoving to position (45, 45, 45)...");
                await printer.Move(45, 45, 45);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                await printer.Shutdown();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}