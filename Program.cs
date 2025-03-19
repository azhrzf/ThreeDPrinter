using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ThreeDimensionPrinter.Hardware;

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

                string filePath = "./move-commands.json";

                if (File.Exists(filePath))
                {

                    Console.WriteLine("Press Enter to start the sequence...");

                    Motor motorX = new Motor("X");

                    motorX.Enable();
                    await motorX.Move(500, 50, 1000);
                    motorX.Stop();
                    motorX.Disable();

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