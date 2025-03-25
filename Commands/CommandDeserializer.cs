using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ThreeDimensionPrinter.Commands
{
    public static class CommandDeserializer
    {
        public static List<PrinterCommand> DeserializeCommands(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var wrapper = JsonSerializer.Deserialize<CommandWrapper>(json, options);
                var jsonCommands = wrapper.Commands;
                var commands = new List<PrinterCommand>();

                foreach (var cmdElement in jsonCommands)
                {
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

        private class CommandWrapper
        {
            public List<JsonElement> Commands { get; set; }
        }
    }
}