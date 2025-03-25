using System;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Hardware;

namespace ThreeDimensionPrinter.Commands
{
    public class CommandExecutor
    {
        private readonly ThreeAxisPrinter _printer;

        public CommandExecutor(ThreeAxisPrinter printer)
        {
            _printer = printer;
        }

        public async Task ExecuteCommand(PrinterCommand command)
        {
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
                    break;

                default:
                    throw new InvalidOperationException($"Unknown command type: {command.CommandType}");
            }
        }
    }
}