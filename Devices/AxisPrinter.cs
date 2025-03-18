namespace ThreeDimensionPrinter
{
    public class AxisPrinter
    {
        public Actuator XActuator { get; private set; }
        public Actuator YActuator { get; private set; }
        public Actuator ZActuator { get; private set; }

        private Motor _xMotor;
        private Motor _yMotor;
        private Motor _zMotor;

        public PrinterConfig Config { get; private set; }

        public AxisPrinter()
        {
            _xMotor = new Motor("X");
            _yMotor = new Motor("Y");
            _zMotor = new Motor("Z");

            XActuator = new Actuator("X", _xMotor);
            YActuator = new Actuator("Y", _yMotor);
            ZActuator = new Actuator("Z", _zMotor);

            Config = new PrinterConfig();

            // Wire up error events
            _xMotor.MotorError += (s, e) => Console.WriteLine("X-axis motor error detected!");
            _yMotor.MotorError += (s, e) => Console.WriteLine("Y-axis motor error detected!");
            _zMotor.MotorError += (s, e) => Console.WriteLine("Z-axis motor error detected!");
        }
    }
}