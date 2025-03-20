using System;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Interfaces;
using ThreeDimensionPrinter.Models;

namespace ThreeDimensionPrinter.Hardware
{
    public class ThreeAxisPrinter
    {
        public Actuator XActuator { get; private set; }
        public Actuator YActuator { get; private set; }
        public Actuator ZActuator { get; private set; }

        private IMotor _xMotor;
        private IMotor _yMotor;
        private IMotor _zMotor;

        public PrinterConfig Config { get; private set; }

        // Constructor that allows specifying vendor types
        public ThreeAxisPrinter(
            MotorFactory.VendorType xVendor,
            MotorFactory.VendorType yVendor,
            MotorFactory.VendorType zVendor)
        {
            _xMotor = MotorFactory.CreateMotor("X", xVendor);
            _yMotor = MotorFactory.CreateMotor("Y", yVendor);
            _zMotor = MotorFactory.CreateMotor("Z", zVendor);

            XActuator = new Actuator("X", _xMotor);
            YActuator = new Actuator("Y", _yMotor);
            ZActuator = new Actuator("Z", _zMotor);

            Config = new PrinterConfig();

            // Wire up error events
            _xMotor.MotorError += (s, e) => Console.WriteLine("X-axis motor error detected!");
            _yMotor.MotorError += (s, e) => Console.WriteLine("Y-axis motor error detected!");
            _zMotor.MotorError += (s, e) => Console.WriteLine("Z-axis motor error detected!");
        }

        public async Task Initialize()
        {
            XActuator.Enable();
            YActuator.Enable();
            ZActuator.Enable();
            Console.WriteLine("\n");

            // Home all axes
            await Task.WhenAll(
                XActuator.Move(Config.Speed, Config.Acceleration, 0),
                YActuator.Move(Config.Speed, Config.Acceleration, 0),
                ZActuator.Move(Config.Speed, Config.Acceleration, 0)
            );
        }

        public async Task Move(double targetX, double targetY, double targetZ)
        {
            // Get current position
            double currentZ = ZActuator.PositionNm;
            double currentX = XActuator.PositionNm;
            double currentY = YActuator.PositionNm;

            Console.WriteLine($"Current position: X:{currentX}, Y:{currentY}, Z:{currentZ}");
            Console.WriteLine($"Target position: X:{targetX}, Y:{targetY}, Z:{targetZ}");

            // Check if X,Y position is changing
            bool xyPositionChanging = Math.Abs(targetX - currentX) > 0.001 || Math.Abs(targetY - currentY) > 0.001;

            // If X,Y position is changing and we're not already at neutral Z
            if (xyPositionChanging && Math.Abs(currentZ) > 0.001)
            {
                // First move Z to neutral position (0)
                Console.WriteLine("Moving Z to neutral position first");
                await ZActuator.Move(Config.Speed, Config.Acceleration, 0);
                currentZ = 0; // Update the current Z position
            }

            // Next move X and Y (simultaneously)
            if (xyPositionChanging)
            {
                Console.WriteLine("Moving X and Y axes");
                await Task.WhenAll(
                    XActuator.Move(Config.Speed, Config.Acceleration, targetX),
                    YActuator.Move(Config.Speed, Config.Acceleration, targetY)
                );
            }

            // Finally move Z to the target position
            if (Math.Abs(targetZ - currentZ) > 0.001)
            {
                Console.WriteLine("Moving Z axis");
                await ZActuator.Move(Config.Speed, Config.Acceleration, targetZ);
            }
        }

        public void SetConfig(double speed, double accel)
        {
            Console.WriteLine($"Setting config: Speed={speed}, Acceleration={accel}");
            Config.Speed = speed;
            Config.Acceleration = accel;
        }

        public void Stop()
        {
            Console.WriteLine("Emergency stop!");
            XActuator.Stop();
            YActuator.Stop();
            ZActuator.Stop();
        }

        public async Task Shutdown()
        {
            // Move to safe position and disable motors
            try
            {
                Console.WriteLine("\n");
                await Move(0, 0, 0);
            }
            catch
            {
                // Ignore errors during shutdown
            }
            finally
            {
                Console.WriteLine("\n");
                XActuator.Disable();
                YActuator.Disable();
                ZActuator.Disable();
                Console.WriteLine("Printer shutdown complete");
            }
        }
    }
}