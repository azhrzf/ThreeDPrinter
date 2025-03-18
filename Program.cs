using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ThreeDimensionPrinter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("3D Printer Controller");
            Console.WriteLine("=====================");

            try
            {
                Console.WriteLine("Initializing printer...");

                string filePath = "./Commands/move-commands.json";

                if (File.Exists(filePath))
                {

                    Console.WriteLine("Press Enter to start the sequence...");

                    Console.WriteLine("Sequence completed successfully!");
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
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}