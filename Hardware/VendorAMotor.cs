using System;
using System.Threading.Tasks;

namespace ThreeDimensionPrinter.Hardware
{
    public class VendorAMotor
    {
        private readonly Motor _motor;

        public VendorAMotor(string name)
        {
            _motor = new Motor(name);
        }

        // Pass-through properties
        public string Name => _motor.Name;
        public int Position => _motor.Position;
        public bool IsEnabled => _motor.IsEnabled;
        public bool IsMoving => _motor.IsMoving;
        public bool HasError => _motor.HasError;

        // Pass-through events
        public event EventHandler<EventArgs> MoveStarted
        {
            add => _motor.MoveStarted += value;
            remove => _motor.MoveStarted -= value;
        }

        public event EventHandler<EventArgs> MoveDone
        {
            add => _motor.MoveDone += value;
            remove => _motor.MoveDone -= value;
        }

        public event EventHandler<EventArgs> MotorError
        {
            add => _motor.MotorError += value;
            remove => _motor.MotorError -= value;
        }

        // Pass-through methods
        public void Enable() => _motor.Enable();
        public void Disable() => _motor.Disable();
        public Task Move(double speed, double acceleration, int destination) =>
            _motor.Move(speed, acceleration, destination);
        public void Stop() => _motor.Stop();
        public Task WaitMoveDone(int timeoutMs) => _motor.WaitMoveDone(timeoutMs);
        public void ClearFault() => _motor.ClearFault();
    }
}