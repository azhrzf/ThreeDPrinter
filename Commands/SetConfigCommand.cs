namespace ThreeDimensionPrinter.Commands
{
    public class SetConfigCommand : PrinterCommand
    {
        public double Speed { get; set; }
        public double Acceleration { get; set; }

        public SetConfigCommand()
        {
            CommandType = PrinterCommandType.SetConfig;
        }
    }
}