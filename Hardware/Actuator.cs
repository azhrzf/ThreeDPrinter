using System.Threading.Tasks;
using ThreeDimensionPrinter.Interfaces;
using ThreeDimensionPrinter.Utils;

namespace ThreeDimensionPrinter.Hardware
{
    public class Actuator
    {
        private readonly IMotor _motor;
        private readonly string _name;

        public double PositionNm => Units.CountToMm(_motor.Position);

        public Actuator(string name, IMotor motor)
        {
            _name = name;
            _motor = motor;
        }

        public void Enable() => _motor.Enable();

        public void Disable() => _motor.Disable();

        public async Task Move(double speed, double accel, double destNm)
        {
            int destCount = Units.MmToCount(destNm);
            double speedCount = Units.MmToCount(speed);
            double accelCount = Units.MmToCount(accel);

            await _motor.Move(speedCount, accelCount, destCount);
        }

        public void Stop() => _motor.Stop();

        public Task WaitMoveDone(int timeoutMs) => _motor.WaitMoveDone(timeoutMs);

        public void ClearFault() => _motor.ClearFault();
    }
}