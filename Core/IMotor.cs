namespace ThreeDimensionPrinter
{
    public interface IMotor
    {
        string Name { get; }
        int Position { get; }
        bool IsEnabled { get; }
        bool IsMoving { get; }
        bool HasError { get; }

        void Enable();
        void Disable();
        Task Move(double speed, double acceleration, int destination);
        void Stop();
        Task WaitMoveDone(int timeoutMs);
        void ClearFault();

        event EventHandler<EventArgs> MoveStarted;
        event EventHandler<EventArgs> MoveDone;
        event EventHandler<EventArgs> MotorError;
    }
}