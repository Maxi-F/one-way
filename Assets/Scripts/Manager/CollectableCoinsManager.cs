using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoinsManager : MonoBehaviour
{
    private int _coinsQuantity;
    private EventManager _eventManager;
    
    void Start()
    {
        CollectableCoin[] collectableCoins = FindObjectsOfType<CollectableCoin>();

        _coinsQuantity = collectableCoins.Length;

        _eventManager = FindObjectOfType<EventManager>();

        if (_eventManager == null)
        {
            Debug.LogError("Event manager not found!");
            return;
        }
        
        _eventManager.SubscribeTo("collectableCoinObtained", OnCoinObtained);
    }

    void OnCoinObtained(Dictionary<string, object> message)
    {
        _coinsQuantity--;

        if (_coinsQuantity == 0)
        {
            _eventManager.TriggerEvent("allCoinsCollected", null);
        }
    }
}
