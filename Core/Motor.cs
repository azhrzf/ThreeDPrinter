namespace ThreeDimensionPrinter
{
    public class Motor : IMotor
    {
        // Events
        public event EventHandler<EventArgs> MoveStarted;
        public event EventHandler<EventArgs> MoveDone;
        public event EventHandler<EventArgs> MotorError;

        // Properties
        public string Name { get; private set; }
        public int Position { get; private set; } // In counts
        public double Speed { get; set; } // In counts/s
        public double Acceleration { get; set; } // In counts/s²

        // Status flags
        public bool IsEnabled { get; private set; }
        public bool IsMoving { get; private set; }
        public bool HasError { get; private set; }

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
        }

        public void Stop()
        {
            if (IsMoving)
            {
                IsMoving = false;
            }
        }

        public async Task WaitMoveDone(int timeoutMs)
        {
            if (!IsMoving) return;
        }

        public void ClearFault()
        {
            if (HasError)
            {
                HasError = false;
            }
        }
    }
}