namespace ThreeDimensionPrinter.Commands
{
    public abstract class PrinterCommand
    {
        public PrinterCommandType CommandType { get; set; }
    }
}