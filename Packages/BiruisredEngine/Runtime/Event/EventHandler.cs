using System;
using System.Collections.Generic;
using System.Reflection;

namespace BiruisredEngine
{
    internal interface IEventHandler
    {
        void Clear();
    }

    /// <summary>
    /// A class that handles all listeners for a specific IEvent event.
    /// </summary>
    /// <typeparam name="T">An IEvent event to be handled.</typeparam>
    internal sealed class EventHandler<T> : IEventHandler where T : IEvent
    {
        private EventCall<T> _event = delegate { };
        private readonly List<Wrapper> _activeWrappers = new();
        private readonly List<Wrapper> _disableWrappers = new();

        /// <summary>
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">A method or delegate with the IEvent type parameter.</param>
        internal void AddListener(EventCall<T> call)
        {
            _event += call;
        }

        /// <summary>
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">A parameterless method or delegate.</param>
        internal void AddListener(Action call)
        {
            Wrapper wrapper;
            if (_disableWrappers.Count == 0)
            {
                wrapper = call;
            }
            else
            {
                wrapper = _disableWrappers[0];
                _disableWrappers.RemoveAt(0);
            }

            wrapper.ReCreate(call);
            _activeWrappers.Add(wrapper);
            _event += wrapper;
        }

        /// <summary>
        /// Removes a registered listener method from the event.
        /// </summary>
        /// <param name="call">A method or delegate with the same IEvent type parameter.</param>
        internal void RemoveListener(EventCall<T> call)
        {
            _event -= call;
        }

        /// <summary>
        /// Removes a registered listener method from the event.
        /// </summary>
        /// <param name="call">A parameterless method or delegate.</param>
        internal void RemoveListener(Action call)
        {
            Wrapper wrapper = null;
            foreach (var item in _activeWrappers)
            {
                if (item.Target != call.Target || item.Method != call.Method) continue;
                wrapper = item;
                break;
            }

            if (wrapper == null) return;
            _event -= wrapper;
            wrapper.Clear();
            _activeWrappers.Remove(wrapper);
            _disableWrappers.Add(wrapper);
        }

        /// <summary>
        /// Invokes/Calls all registered listeners method of the event.
        /// </summary>
        /// <param name="evt">The data passed to the invoked listeners.</param>
        internal void Invoke(IEvent evt)
        {
            _event((T)evt);
        }

        /// <summary>
        /// Removes all registered listeners method of the event.
        /// </summary>
        public void Clear()
        {
            _event = delegate { };
            foreach (var wrapper in _activeWrappers)
            {
                wrapper.Clear();
                _disableWrappers.Add(wrapper);
            }

            _activeWrappers.Clear();
        }

        /// <summary>
        /// Wrapper class to hold a listener method.
        /// </summary>
        private class Wrapper
        {
            private Wrapper(Action call)
            {
                ReCreate(call);
            }

            internal MethodInfo Method;
            internal object Target;
            private EventCall<T> _event;

            internal void ReCreate(Action call)
            {
                Method = call.Method;
                Target = call.Target;
                _event = _ => call();
            }

            public static implicit operator EventCall<T>(Wrapper wrapper)
            {
                return wrapper._event;
            }

            public static implicit operator Wrapper(Action action)
            {
                return new Wrapper(action);
            }

            internal void Clear()
            {
                Target = null;
                Method = null;
                _event = null;
            }
        }
    }
}