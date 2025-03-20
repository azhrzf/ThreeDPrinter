namespace ThreeDimensionPrinter.Commands
{
    public class MoveCommand : PrinterCommand
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MoveCommand()
        {
            CommandType = PrinterCommandType.Move;
        }
    }
}