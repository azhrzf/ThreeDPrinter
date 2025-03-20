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
                printer.Initialize();

                Console.WriteLine("Moving to position (10, 0, 0)...");
                await printer.Move(10, 0, 0);

                Console.WriteLine("Moving to position (10, 10, 0)...");
                await printer.Move(10, 10, 0);

                Console.WriteLine("Moving to position (10, 10, 5)...");
                await printer.Move(10, 10, 5);

                Console.WriteLine("Moving to position (0, 0, 0)...");
                await printer.Move(0, 0, 0);
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