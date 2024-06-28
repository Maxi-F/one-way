using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class CollectableCoin : WithDebugRemover
{
    [SerializeField] private string collectableCoinEvent = "collectableCoinObtained";
    
    private void Start()
    {
        RemoveDebug();
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.Instance.TriggerEvent(collectableCoinEvent, null);
        
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
