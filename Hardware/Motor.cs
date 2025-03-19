using System;
using System.Threading;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Interfaces;

namespace ThreeDimensionPrinter.Hardware
{
    public class Motor : IMotor
    {
        // Properties
        public string Name { get; private set; }
        public int Position { get; private set; } // In counts
        public double Speed { get; set; } // In counts/s
        public double Acceleration { get; set; } // In counts/s²

        // Status flags
        public bool IsEnabled { get; private set; }
        public bool IsMoving { get; private set; }
        public bool HasError { get; private set; }

        // Events
        public event EventHandler<EventArgs> MoveStarted;
        public event EventHandler<EventArgs> MoveDone;
        public event EventHandler<EventArgs> MotorError;

        public Motor(string name)
        {
            Name = name;
            Position = 0;
            IsEnabled = false;
            IsMoving = false;
            HasError = false;
            Speed = 1000;
            Acceleration = 100;
        }

        public void Enable()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"Enabling {Name} motor");
                IsEnabled = true;
                HasError = false;
            }
        }

        public void Disable()
        {
            if (IsEnabled)
            {
                if (IsMoving)
                {
                    Stop();
                }
                Console.WriteLine($"Disabling {Name} motor");
                IsEnabled = false;
            }
        }

        public async Task Move(double speed, double accel, int destination)
        {
            if (!IsEnabled)
            {
                throw new InvalidOperationException($"{Name} motor is not enabled");
            }

            if (IsMoving)
            {
                Stop();
            }

            IsMoving = true;
            Speed = speed;
            Acceleration = accel;

            Console.WriteLine($"{Name} moving to {destination} at speed {speed}, acceleration {accel}");

            try
            {

            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"{Name} movement cancelled");
            }
            finally
            {
                IsMoving = false;
                // Add Event to notify move done
            }
        }

        public void Stop()
        {
            if (IsMoving)
            {
                Console.WriteLine($"Stopping {Name} motor");
                // Add Event to stop motor
                IsMoving = false;
            }
        }

        public async Task WaitMoveDone(int timeoutMs)
        {
            if (!IsMoving) return;

            // Implement timeout
        }

        public void ClearFault()
        {
            if (HasError)
            {
                Console.WriteLine($"Clearing fault on {Name} motor");
                HasError = false;
            }
        }
    }
}