using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoinsManager : MonoBehaviour
{
    private int _coinsObtained = 0;
    private CollectableCoin[] _collectableCoins;
    private EventManager _eventManager;
    
    void Start()
    {
        _collectableCoins = FindObjectsOfType<CollectableCoin>();
        
        _eventManager = FindObjectOfType<EventManager>();

        if (_eventManager == null)
        {
            Debug.LogError("Event manager not found!");
            return;
        }
        
        _eventManager.SubscribeTo("collectableCoinObtained", OnCoinObtained);
        _eventManager.SubscribeTo("playerDeath", OnReset);
    }

    void OnCoinObtained(Dictionary<string, object> message)
    {
        _coinsObtained++;

        if (_coinsObtained == _collectableCoins.Length)
        {
            _eventManager.TriggerEvent("allCoinsCollected", null);
        }
    }

    void OnReset(Dictionary<string, object> message)
    {
        foreach (var collectableCoin in _collectableCoins)
        {
            collectableCoin.Reset();
        }

        _coinsObtained = 0;
    }
}
