namespace ThreeDimensionPrinter.Commands
{
    // Base class for all printer commands
    // It uses the PrinterCommandType enumeration defined in the other file.
    
    public abstract class PrinterCommand
    {
        public PrinterCommandType CommandType { get; set; }
    }
}