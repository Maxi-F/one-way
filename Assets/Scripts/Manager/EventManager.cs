using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<Dictionary<string, object>>> _events;

    private void Awake()
    {
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
