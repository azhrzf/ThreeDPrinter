namespace ThreeDimensionPrinter.Commands
{
    public class StopCommand : PrinterCommand
    {
        public StopCommand()
        {
            CommandType = PrinterCommandType.Stop;
        }
    }
}