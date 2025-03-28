using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Hardware;

namespace ThreeDimensionPrinter.Commands
{
    public class Sequence
    {
        private readonly ThreeAxisPrinter _printer;
        private List<PrinterCommand> _commands;
        private bool _isRunning;
        private CancellationTokenSource _sequenceCts;
        private readonly CommandExecutor _executor;

        public Sequence(ThreeAxisPrinter printer)
        {
            _printer = printer;
            _commands = new List<PrinterCommand>();
            _isRunning = false;
            _executor = new CommandExecutor(_printer);
        }

        public async Task LoadSequence(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);
            _commands = CommandDeserializer.DeserializeCommands(json);
            Console.WriteLine($"Loaded {_commands.Count} commands");
            Console.WriteLine("\n");
        }

        public async Task Start()
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("Sequence is already running");
            }

            if (_commands.Count == 0)
            {
                Console.WriteLine("No commands to execute");
                return;
            }

            _isRunning = true;
            _sequenceCts = new CancellationTokenSource();

            try
            {
                Console.WriteLine("Starting sequence execution");

                for (int i = 0; i < _commands.Count; i++)
                {
                    _sequenceCts.Token.ThrowIfCancellationRequested();

                    var command = _commands[i];
                    Console.WriteLine($"Executing command {i + 1}/{_commands.Count}: {command.CommandType}");

                    await _executor.ExecuteCommand(command);

                    Console.WriteLine($"Command {i + 1} completed");
                    Console.WriteLine("\n");
                }

                Console.WriteLine("Sequence completed");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Sequence execution cancelled");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing sequence: {ex.Message}");
                throw;
            }
            finally
            {
                _isRunning = false;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                Console.WriteLine("Stopping sequence execution");
                _sequenceCts?.Cancel();
                _printer.Stop();
                _isRunning = false;
            }
        }
    }
}