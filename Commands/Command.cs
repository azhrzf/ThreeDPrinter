namespace ThreeDimensionPrinter
{
    public enum PrinterCommandType
    {
        Move,
        SetConfig,
        Stop
    }

    public abstract class PrinterCommand
    {
        public PrinterCommandType CommandType { get; set; }
    }

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

    // Conf Command. stores speed and acceleration parameters for the 3D printer
    public class SetConfigCommand : PrinterCommand
    {
        public double Speed { get; set; }
        public double Acceleration { get; set; }

        public SetConfigCommand()
        {
            CommandType = PrinterCommandType.SetConfig;
        }
    }

    public class StopCommand : PrinterCommand
    {
        public StopCommand()
        {
            CommandType = PrinterCommandType.Stop;
        }
    }


}