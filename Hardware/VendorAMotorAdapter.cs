using System;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Interfaces;

namespace ThreeDimensionPrinter.Hardware
{
    public class VendorAMotorAdapter : IMotor
    {
        private readonly VendorAMotor _vendorAMotor;

        public VendorAMotorAdapter(string name)
        {
            _vendorAMotor = new VendorAMotor(name);
        }

        // Pass-through properties
        public string Name => _vendorAMotor.Name;
        public int Position => _vendorAMotor.Position;
        public bool IsEnabled => _vendorAMotor.IsEnabled;
        public bool IsMoving => _vendorAMotor.IsMoving;
        public bool HasError => _vendorAMotor.HasError;

        // Pass-through events
        public event EventHandler<EventArgs> MoveStarted
        {
            add => _vendorAMotor.MoveStarted += value;
            remove => _vendorAMotor.MoveStarted -= value;
        }

        public event EventHandler<EventArgs> MoveDone
        {
            add => _vendorAMotor.MoveDone += value;
            remove => _vendorAMotor.MoveDone -= value;
        }

        public event EventHandler<EventArgs> MotorError
        {
            add => _vendorAMotor.MotorError += value;
            remove => _vendorAMotor.MotorError -= value;
        }

        // Pass-through methods - no parameter reordering needed for VendorA
        public void Enable() => _vendorAMotor.Enable();
        public void Disable() => _vendorAMotor.Disable();
        public Task Move(double speed, double acceleration, int destination) =>
            _vendorAMotor.Move(speed, acceleration, destination);
        public void Stop() => _vendorAMotor.Stop();
        public Task WaitMoveDone(int timeoutMs) => _vendorAMotor.WaitMoveDone(timeoutMs);
        public void ClearFault() => _vendorAMotor.ClearFault();
    }
}