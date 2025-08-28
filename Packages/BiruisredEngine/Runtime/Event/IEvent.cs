using System.Collections;

namespace BiruisredEngine
{
    /// <summary>
    /// Base interface for ikanAsin event structs. Create the struct to hold the parameter data of the event.
    /// </summary>
    public interface IEvent { }

    public delegate void EventCall<in T>(T evt) where T : IEvent;

    /// <summary>
    /// A coroutine version of IEvent.
    /// </summary>
    public interface IEnumeratorEvent { }

    public delegate IEnumerator EnumeratorEventCall<in T>(T evt) where T : IEnumeratorEvent;

    public delegate IEnumerator EnumeratorEventCall();

    public delegate void EnumeratorAction<in T>(T evt) where T : IEnumeratorEvent;

    public delegate void EnumeratorAction();

    /// <summary>
    /// Add this interface to IEvent or IEnumeratorEvent structs to make them persistent.
    /// </summary>
    public interface IPersistenceEvent { }
    
}