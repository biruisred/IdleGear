using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BiruisredEngine
{
    public static class EVENT
    {
        private static readonly Dictionary<Type, IEventHandler> _baseHandlers = new();
        private static readonly Dictionary<Type, IEnumeratorEventHandler> _enumeratorHandlers = new();

        internal static void Create()
        {
            _baseHandlers.Clear();
            _enumeratorHandlers.Clear();
        }

        internal static void CollectEventFromType(Type type)
        {
            if (type.IsInterface) return;
            CheckEvent();
            CheckEnumeratorEvent();

            void CheckEvent()
            {
                if (!typeof(IEvent).IsAssignableFrom(type)) return;
                var targetType = typeof(EventHandler<>).MakeGenericType(type);
                if (Activator.CreateInstance(targetType) is not IEventHandler eventHandler) return;
                _baseHandlers.Add(type, eventHandler);
            }

            void CheckEnumeratorEvent()
            {
                if (!typeof(IEnumeratorEvent).IsAssignableFrom(type)) return;
                var targetType = typeof(EnumeratorEventHandler<>).MakeGenericType(type);
                if (Activator.CreateInstance(targetType) is not IEnumeratorEventHandler eventHandler) return;
                _enumeratorHandlers.Add(type, eventHandler);
            }
        }

        /// <summary>
        /// Removes all listeners from all IEvent and IEnumeratorEvent events.
        /// </summary>
        public static void Clear()
        {
            foreach (var (_, handler) in _baseHandlers)
            {
                handler.Clear();
            }

            foreach (var (_, handler) in _enumeratorHandlers)
            {
                handler.Clear();
            }
        }

        /// <summary>
        /// Registers a listener method to an IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event to listen to.</typeparam>
        /// <param name="call">A method or delegate with the same IEvent type parameter.</param>
        public static void AddListener<T>(EventCall<T> call) where T : IEvent
        {
            GetHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Registers a listener method to an IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event to listen to.</typeparam>
        /// <param name="call">A parameterless method or delegate.</param>
        public static void AddListener<T>(Action call) where T : IEvent
        {
            GetHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Registers a listener method to an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to listen to.</typeparam>
        /// <param name="call">An IEnumerator method or delegate with the same IEnumeratorEvent type parameter.</param>
        public static void AddListener<T>(EnumeratorEventCall<T> call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Registers a listener method to an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to listen to.</typeparam>
        /// <param name="call">A parameterless IEnumerator method or delegate.</param>
        public static void AddListener<T>(EnumeratorEventCall call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Registers a listener method to an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to listen to.</typeparam>
        /// <param name="call">A method or delegate with the same IEnumeratorEvent type parameter.</param>
        public static void AddListener<T>(EnumeratorAction<T> call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Registers a listener method to an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to listen to.</typeparam>
        /// <param name="call">A parameterless method or delegate.</param>
        public static void AddListener<T>(EnumeratorAction call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().AddListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the listener is removed from.</typeparam>
        /// <param name="call">A method or delegate with the same IEvent type parameter.</param>
        public static void RemoveListener<T>(EventCall<T> call) where T : IEvent
        {
            GetHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the method/delegate is removed from.</typeparam>
        /// <param name="call">A parameterless method or delegate.</param>
        public static void RemoveListener<T>(Action call) where T : IEvent
        {
            GetHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the method/delegate is removed from.</typeparam>
        /// <param name="call">An IEnumerator method or delegate with the same IEnumeratorEvent type parameter.</param>
        public static void RemoveListener<T>(EnumeratorEventCall<T> call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the method/delegate is removed from.</typeparam>
        /// <param name="call">A parameterless IEnumerator method or delegate.</param>
        public static void RemoveListener<T>(EnumeratorEventCall call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the method/delegate is removed from.</typeparam>
        /// <param name="call">A method or delegate with the same IEnumeratorEvent type parameter.</param>
        public static void RemoveListener<T>(EnumeratorAction<T> call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Removes a registered listener method from an IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event from which the method/delegate is removed from.</typeparam>
        /// <param name="call">A parameterless method or delegate.</param>
        public static void RemoveListener<T>(EnumeratorAction call) where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().RemoveListener(call);
        }

        /// <summary>
        /// Invokes/Calls all registered listener method of the IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event to be invoked.</typeparam>
        /// <param name="event">The data passed to the invoked listeners.</param>
        public static void Invoke<T>(T @event) where T : IEvent
        {
            GetHandler<T>().Invoke(@event);
        }

        /// <summary>
        /// Invokes/Calls all registered listener method of the IEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event to be invoked.</typeparam>
        public static void Invoke<T>() where T : IEvent, new()
        {
            GetHandler<T>().Invoke(new T());
        }

        /// <summary>
        /// Invokes/Calls all registered method/delegate of the IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to be invoked.</typeparam>
        /// <param name="event">The data passed to the invoked listeners.</param>
        public static IEnumerator YieldInvoke<T>(T @event) where T : IEnumeratorEvent
        {
            return GetEnumeratorHandler<T>().Invoke(@event);
        }

        /// <summary>
        /// Invokes/Calls all registered method/delegate of the IEnumeratorEvent type event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to be invoked.</typeparam>
        public static IEnumerator YieldInvoke<T>() where T : IEnumeratorEvent, new()
        {
            return GetEnumeratorHandler<T>().Invoke(new T());
        }

        /// <summary>
        /// Removes all listeners from a single IEvent event.
        /// </summary>
        /// <typeparam name="T">The IEvent type event to be cleared.</typeparam>
        public static void Clear<T>() where T : IEvent
        {
            GetHandler<T>().Clear();
        }

        /// <summary>
        /// Removes all listeners from a single IEnumeratorEvent event.
        /// </summary>
        /// <typeparam name="T">The IEnumeratorEvent type event to be cleared.</typeparam>
        public static void YieldClear<T>() where T : IEnumeratorEvent
        {
            GetEnumeratorHandler<T>().Clear();
        }

        private static EventHandler<T> GetHandler<T>() where T : IEvent
        {
            return _baseHandlers[typeof(T)] as EventHandler<T>;
        }

        private static EnumeratorEventHandler<T> GetEnumeratorHandler<T>() where T : IEnumeratorEvent
        {
            return _enumeratorHandlers[typeof(T)] as EnumeratorEventHandler<T>;
        }
    }

    public static class Extension { }
}