using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BiruisredEngine
{
    internal interface IEnumeratorEventHandler
    {
        void Clear();
    }

    /// <summary>
    /// A class that handles all listeners for a specific IEnumeratorEvent event.
    /// </summary>
    /// <typeparam name="T">An IEnumeratorEvent event to be handled.</typeparam>
    internal sealed class EnumeratorEventHandler<T> : IEnumeratorEventHandler where T : IEnumeratorEvent
    {
        public EnumeratorEventHandler()
        {
            _event = null;
            _event += Empty;
        }

        private EnumeratorEventCall<T> _event;
        private readonly List<Wrapper> _activeWrappers = new();
        private readonly List<Wrapper> _disableWrappers = new();

        /// <summary>
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">An IEnumerator method or delegate with the IEnumeratorEvent type parameter.</param>
        internal void AddListener(EnumeratorEventCall<T> call)
        {
            _event += call;
        }

        /// <summary>
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">A parameterless IEnumerator method or delegate.</param>
        internal void AddListener(EnumeratorEventCall call)
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
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">A method or delegate with the same IEnumeratorEvent type parameter.</param>
        internal void AddListener(EnumeratorAction<T> call)
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
        /// Registers a listener method to the event.
        /// </summary>
        /// <param name="call">A parameterless method or delegate.</param>
        internal void AddListener(EnumeratorAction call)
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
        /// <param name="call">An IEnumerator method or delegate with the IEnumeratorEvent type parameter.</param>
        internal void RemoveListener(EnumeratorEventCall<T> call)
        {
            _event -= call;
        }

        /// <summary>
        /// Removes a registered listener method from the event.
        /// </summary>
        /// <param name="call">A parameterless IEnumerator method or delegate.</param>
        internal void RemoveListener(EnumeratorEventCall call)
        {
            Wrapper wrapper = null;
            foreach (var item in _activeWrappers)
            {
                if (item.Target != call.Target || item.Method != call.Method) continue;
                wrapper = item;
                break;
            }

            if (wrapper == null)
            {
                return;
            }

            _event -= wrapper;
            wrapper.Clear();
            _activeWrappers.Remove(wrapper);
            _disableWrappers.Add(wrapper);
        }

        /// <summary>
        /// Removes a registered listener method from the event.
        /// </summary>
        /// <param name="call">A method or delegate with the same IEnumeratorEvent type parameter.</param>
        internal void RemoveListener(EnumeratorAction<T> call)
        {
            Wrapper wrapper = null;
            foreach (var item in _activeWrappers)
            {
                if (item.Target != call.Target || item.Method != call.Method) continue;
                wrapper = item;
                break;
            }

            if (wrapper == null)
            {
                return;
            }

            _event -= wrapper;
            wrapper.Clear();
            _activeWrappers.Remove(wrapper);
            _disableWrappers.Add(wrapper);
        }

        /// <summary>
        /// Removes a registered listener method from the event.
        /// </summary>
        /// <param name="call">A parameterless method or delegate.</param>
        internal void RemoveListener(EnumeratorAction call)
        {
            Wrapper wrapper = null;
            foreach (var item in _activeWrappers)
            {
                if (item.Target != call.Target || item.Method != call.Method)
                    continue;
                wrapper = item;
                break;
            }

            if (wrapper == null)
            {
                return;
            }

            _event -= wrapper;
            wrapper.Clear();
            _activeWrappers.Remove(wrapper);
            _disableWrappers.Add(wrapper);
        }

        /// <summary>
        /// Invokes/Calls all registered listeners method of the event.
        /// </summary>
        /// <param name="evt">The data passed to the invoked listeners.</param>
        internal IEnumerator Invoke(IEnumeratorEvent evt)
        {
            foreach (var delegate1 in _event.GetInvocationList())
            {
                var @delegate = (EnumeratorEventCall<T>)delegate1;
                yield return @delegate.Invoke((T)evt);
            }
        }

        /// <summary>
        /// Removes all registered listeners method of the event.
        /// </summary>
        public void Clear()
        {
            _event = null;
            _event += Empty;
            foreach (var wrapper in _activeWrappers)
            {
                wrapper.Clear();
                _disableWrappers.Add(wrapper);
            }

            _activeWrappers.Clear();
        }


        /// <summary>
        /// An empty IEnumerator method for placeholder.
        /// </summary>
        /// <param name="evt">The data passed to the invoked listeners.</param>
        private IEnumerator Empty(T evt)
        {
            yield break;
        }

        /// <summary>
        /// Wrapper class to hold a listener method.
        /// </summary>
        private class Wrapper
        {
            private Wrapper(EnumeratorEventCall call)
            {
                ReCreate(call);
            }

            private Wrapper(EnumeratorAction call)
            {
                ReCreate(call);
            }

            private Wrapper(EnumeratorAction<T> call)
            {
                ReCreate(call);
            }

            internal MethodInfo Method;
            internal object Target;
            private EnumeratorEventCall<T> _event;

            internal void ReCreate(EnumeratorEventCall call)
            {
                Method = call.Method;
                Target = call.Target;
                _event = _ => call();
            }

            internal void ReCreate(EnumeratorAction call)
            {
                Method = call.Method;
                Target = call.Target;
                _event = _ => Do(call);
            }

            internal void ReCreate(EnumeratorAction<T> call)
            {
                Method = call.Method;
                Target = call.Target;
                _event = evt => Do(evt, call);
            }

            internal void Clear()
            {
                Target = null;
                Method = null;
                _event = null;
            }

            private IEnumerator Do(EnumeratorAction action)
            {
                action();
                yield break;
            }

            private IEnumerator Do(T evt, EnumeratorAction<T> action)
            {
                action(evt);
                yield break;
            }

            public static implicit operator EnumeratorEventCall<T>(Wrapper wrapper)
            {
                return wrapper._event;
            }

            public static implicit operator Wrapper(EnumeratorEventCall eventCall)
            {
                return new Wrapper(eventCall);
            }

            public static implicit operator Wrapper(EnumeratorAction action)
            {
                return new Wrapper(action);
            }

            public static implicit operator Wrapper(EnumeratorAction<T> action)
            {
                return new Wrapper(action);
            }
        }
    }
}