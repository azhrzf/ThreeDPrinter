using System;
using System.Threading;
using System.Threading.Tasks;
using ThreeDimensionPrinter.Interfaces;

namespace ThreeDimensionPrinter.Hardware
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

        private readonly Random _random = new Random(); // For simulation
        private CancellationTokenSource _moveCts;

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

            _moveCts = new CancellationTokenSource();
            MoveStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                // Simulate movement time based on distance and speed
                int distance = Math.Abs(destination - Position);
                int simulatedTimeMs = (int)(distance / speed * 1000);

                // Add some randomness for simulation
                simulatedTimeMs += _random.Next(0, 100);

                await Task.Delay(simulatedTimeMs, _moveCts.Token);
                Position = destination;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"{Name} movement cancelled");
            }
            finally
            {
                IsMoving = false;
                MoveDone?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (IsMoving)
            {
                Console.WriteLine($"Stopping {Name} motor");
                _moveCts?.Cancel();
                IsMoving = false;
            }
        }

        public async Task WaitMoveDone(int timeoutMs)
        {
            if (!IsMoving) return;

            // TimeoutTask will complete after the specified timeout period
            var timeoutTask = Task.Delay(timeoutMs);

            // Manual task that will be completed when the motor finishes moving
            var tcs = new TaskCompletionSource<bool>();

            // This method is called when the MoveDone event occurs.
            void OnMoveDone(object sender, EventArgs e)
            {
                // Unsubscribes from the event to prevent multiple calls.
                MoveDone -= OnMoveDone;
                tcs.TrySetResult(true);
            }

            MoveDone += OnMoveDone;

            if (await Task.WhenAny(tcs.Task, timeoutTask) == timeoutTask)
            {
                MoveDone -= OnMoveDone;
                throw new TimeoutException($"{Name} movement did not complete within timeout");
            }
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