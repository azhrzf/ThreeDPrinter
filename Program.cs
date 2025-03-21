using System;
using System.IO;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Commands;
using ThreeDimensionPrinter.Hardware;

namespace ThreeDimensionPrinter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\n");
            Console.WriteLine("3D Printer Controller");
            Console.WriteLine("=====================");
            Console.WriteLine("\n");

            var printer = new ThreeAxisPrinter();
            var sequencer = new Sequence(printer);

            try
            {
                Console.WriteLine("Initializing printer...");
                await printer.Initialize();

                string filePath = "./move-commands.json";

                if (File.Exists(filePath))
                {
                    await sequencer.LoadSequence(filePath);
                    await sequencer.Start();

                    Console.WriteLine("Sequence completed successfully!");
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.WriteLine("File not found!");
                }
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