using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, Action<Dictionary<string, object>>> _events;

        private static EventManager _instance;
        public static EventManager Instance
        {
            // checks with null because if object is destroyed it returns true but object is not null.
            get { return _instance == null ? null : _instance; }
            private set { _instance = value; }
        }
    
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        
            _events = new Dictionary<string, Action<Dictionary<string, object>>>();
        }
    
        /// <summary>
        /// Subscribes to an event on the event manager.
        /// </summary>
        /// <param name="anEventName">The event name key.</param>
        /// <param name="listener">
        /// The listener of the event. This listener must have a dictionary parameter, with which
        /// you will be able to receive any data and parse it from the object type.
        /// </param>
        public void SubscribeTo(string anEventName, Action<Dictionary<string, object>> listener) {
            Action<Dictionary<string, object>> anEvent;
        
            if (_events.TryGetValue(anEventName, out anEvent))
            {
                anEvent += listener;
                _events[anEventName] = anEvent;
            }
            else
            {
                anEvent = listener;
                _events.Add(anEventName, anEvent);
            }
        }

        /// <summary>
        /// Unsubscribes from the event with the event name key.
        /// </summary>
        /// <param name="eventName">The event name key</param>
        /// <param name="listener">The listener of the event.</param>
        public void UnsubscribeTo(string eventName, Action<Dictionary<string, object>> listener)
        {
            Action<Dictionary<string, object>> anEvent;
            if (_events.TryGetValue(eventName, out anEvent)) {
                anEvent -= listener;
                _events[eventName] = anEvent;
            }
        }

        /// <summary>
        /// Triggers an event using an event name.
        /// </summary>
        /// <param name="eventName">The event name key</param>
        /// <param name="message">The message to send to the event, which has a format of a dictionary with key values.</param>
        public void TriggerEvent(string eventName, Dictionary<string, object> message)
        {
            Action<Dictionary<string, object>> anEvent = null;
            if (_events.TryGetValue(eventName, out anEvent)) {
                anEvent?.Invoke(message);
            }
        }
    }
}
