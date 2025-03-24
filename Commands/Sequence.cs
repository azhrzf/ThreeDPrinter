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
        // _printer is an instance of the ThreeAxisPrinter class.
        private readonly ThreeAxisPrinter _printer;
        private List<PrinterCommand> _commands;
        private bool _isRunning;
        // A CancellationTokenSource is a class in C# that  generate cancellation tokens, which can be used to signal to operations that they should be canceled.
        // A cancellation token source for stopping operations.
        private CancellationTokenSource _sequenceCts;

        public Sequence(ThreeAxisPrinter printer)
        {
            _printer = printer;
            _commands = new List<PrinterCommand>();
            _isRunning = false;
        }

        // Asynchronously loads a sequence of commands from a file
        public async Task LoadSequence(string filePath)
        {
            // Asynchronously opens a text file, reads all the text in the file, and then closes the file.
            string json = await File.ReadAllTextAsync(filePath);
            _commands = DeserializeCommands(json);
            Console.WriteLine($"Loaded {_commands.Count} commands");
            Console.WriteLine("\n");
        }

        private List<PrinterCommand> DeserializeCommands(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                // Deserialize the JSON into a wrapper object that contains the commands array
                var wrapper = JsonSerializer.Deserialize<CommandWrapper>(json, options);
                var jsonCommands = wrapper.Commands;
                var commands = new List<PrinterCommand>();

                foreach (var cmdElement in jsonCommands)
                {
                    // Get the command type
                    if (cmdElement.TryGetProperty("type", out JsonElement typeElement))
                    {
                        var typeStr = typeElement.GetString();
                        if (Enum.TryParse<PrinterCommandType>(typeStr, true, out var cmdType))
                        {
                            PrinterCommand cmd = null;

                            switch (cmdType)
                            {
                                case PrinterCommandType.Move:
                                    var moveCmd = new MoveCommand();
                                    if (cmdElement.TryGetProperty("params", out JsonElement moveParamsElement))
                                    {
                                        if (moveParamsElement.TryGetProperty("x", out var xElem)) moveCmd.X = xElem.GetDouble();
                                        if (moveParamsElement.TryGetProperty("y", out var yElem)) moveCmd.Y = yElem.GetDouble();
                                        if (moveParamsElement.TryGetProperty("z", out var zElem)) moveCmd.Z = zElem.GetDouble();
                                    }
                                    cmd = moveCmd;
                                    break;

                                case PrinterCommandType.SetConfig:
                                    var configCmd = new SetConfigCommand();
                                    if (cmdElement.TryGetProperty("params", out JsonElement configParamsElement))
                                    {
                                        if (configParamsElement.TryGetProperty("speed", out var speedElem)) configCmd.Speed = speedElem.GetDouble();
                                        if (configParamsElement.TryGetProperty("acceleration", out var accelElem)) configCmd.Acceleration = accelElem.GetDouble();
                                    }
                                    cmd = configCmd;
                                    break;

                                case PrinterCommandType.Stop:
                                    cmd = new StopCommand();
                                    break;
                            }

                            if (cmd != null)
                            {
                                commands.Add(cmd);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Unknown command type: {typeStr}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Command type not found in JSON element");
                    }
                }

                return commands;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                Console.WriteLine($"JSON: {json}");
                return new List<PrinterCommand>();
            }
        }

        // Wrapper class to match the JSON structure
        private class CommandWrapper
        {
            public List<JsonElement> Commands { get; set; }
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

                    switch (command.CommandType)
                    {
                        case PrinterCommandType.Move:
                            var moveCmd = (MoveCommand)command;
                            await _printer.Move(moveCmd.X, moveCmd.Y, moveCmd.Z);
                            break;

                        case PrinterCommandType.SetConfig:
                            var configCmd = (SetConfigCommand)command;
                            _printer.SetConfig(configCmd.Speed, configCmd.Acceleration);
                            break;

                        case PrinterCommandType.Stop:
                            _printer.Stop();
                            return; // Exit the sequence if we get a stop command
                    }

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