using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : WithDebugRemover
{
    private EventManager _eventManager;
    private void Start()
    {
        RemoveDebug();
        
        _eventManager = FindObjectOfType<EventManager>();

        if (_eventManager == null)
        {
            Debug.LogError("Event manager is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _eventManager.TriggerEvent("collectableCoinObtained", null);
        
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
