namespace ThreeDimensionPrinter
{
    public class VendorBMotorAdapter : IMotor
    {
        private readonly VendorBMotor _vendorBMotor;

        public VendorBMotorAdapter(string name)
        {
            _vendorBMotor = new VendorBMotor(name);
        }

        // Pass-through properties
        public string Name => _vendorBMotor.Name;
        public int Position => _vendorBMotor.Position;
        public bool IsEnabled => _vendorBMotor.IsEnabled;
        public bool IsMoving => _vendorBMotor.IsMoving;
        public bool HasError => _vendorBMotor.HasError;

        // Pass-through events
        public event EventHandler<EventArgs> MoveStarted
        {
            add => _vendorBMotor.MoveStarted += value;
            remove => _vendorBMotor.MoveStarted -= value;
        }

        public event EventHandler<EventArgs> MoveDone
        {
            add => _vendorBMotor.MoveDone += value;
            remove => _vendorBMotor.MoveDone -= value;
        }

        public event EventHandler<EventArgs> MotorError
        {
            add => _vendorBMotor.MotorError += value;
            remove => _vendorBMotor.MotorError -= value;
        }

        // Methods with parameter reordering for VendorB
        public void Enable() => _vendorBMotor.Enable();
        public void Disable() => _vendorBMotor.Disable();

        // Adapt the standard interface to VendorB's parameter order
        public Task Move(double speed, double acceleration, int destination) =>
            _vendorBMotor.Move(acceleration, destination, speed);

        public void Stop() => _vendorBMotor.Stop();
        public Task WaitMoveDone(int timeoutMs) => _vendorBMotor.WaitMoveDone(timeoutMs);
        public void ClearFault() => _vendorBMotor.ClearFault();
    }
}