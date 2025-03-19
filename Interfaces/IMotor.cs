using System;
using System.Threading.Tasks;

namespace ThreeDimensionPrinter.Interfaces
{

    /*
    KEEP IN MIND:
    An async method usually returns Task or Task<T> (but can also return void in some cases).
    Task: Represents an asynchronous operation. It is part of the System.Threading.Tasks namespace and is used to perform work asynchronously, allowing the application to continue executing other code while waiting for the task to complete.
    */

    /*
    KEEP IN MIND:
    An event in C# is a mechanism that allows a class to notify subscribers when something happens. It follows the Observer pattern, where multiple objects (subscribers) can listen for changes in another object (publisher).
    */

    /*
    IMotor interface
    Puropose: Defines a standard contract for all motor implementations
    Uses: Establishes required methods for any motor implementation, Enables motor interchangeability through polymorphism, Facilitates the adapter pattern implementation

    */

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


        /* This event is triggered when a move operation starts. Subscribers can handle this event to perform actions when the motor begins moving.
        */
        event EventHandler<EventArgs> MoveStarted;

        // This event is triggered when a move operation completes. Subscribers can handle this event to perform actions when the motor finishes moving.
        event EventHandler<EventArgs> MoveDone;

        // This event is triggered when an error occurs in the motor. Subscribers can handle this event to perform actions when a motor error is detected.
        event EventHandler<EventArgs> MotorError;
    }
}