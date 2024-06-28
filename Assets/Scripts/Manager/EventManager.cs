using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, Action<Dictionary<string, object>>> _events;
        public static EventManager Instance { get; private set; }
    
    
    
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

        public void UnsubscribeTo(string eventName, Action<Dictionary<string, object>> listener)
        {
            Action<Dictionary<string, object>> anEvent;
            if (_events.TryGetValue(eventName, out anEvent)) {
                anEvent -= listener;
                _events[eventName] = anEvent;
            }
        }

        public void TriggerEvent(string eventName, Dictionary<string, object> message)
        {
            Action<Dictionary<string, object>> anEvent = null;
            if (_events.TryGetValue(eventName, out anEvent)) {
                anEvent.Invoke(message);
            }
        }
    }
}
