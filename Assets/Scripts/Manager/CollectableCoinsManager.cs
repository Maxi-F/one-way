using System;
using PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class CollectableCoinsManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private string collectableCoinObtainedEvent = "collectableCoinObtained";
    [SerializeField] private string playerDeathEvent = "playerDeath";
    [SerializeField] private string allCoinsCollectedEvent = "allCoinsCollected";
    [SerializeField] private string collectableCoinsCountEvent = "collectableCoinsCount";
    
    private int _coinsObtained = 0;
    private CollectableCoin[] _collectableCoins;
    void Start()
    {
        _collectableCoins = FindObjectsOfType<CollectableCoin>();
        
        foreach (var collectableCoin in _collectableCoins)
        {
            CoinLookAt coinLookAt = collectableCoin.GetComponentInChildren<CoinLookAt>();
            coinLookAt.SetTransform(player.transform);
        }
        
        EventManager.Instance.TriggerEvent(collectableCoinsCountEvent, new Dictionary<string, object>() { { "value", _collectableCoins.Length } });
        EventManager.Instance.SubscribeTo(collectableCoinObtainedEvent, OnCoinObtained);
        EventManager.Instance.SubscribeTo(playerDeathEvent, OnReset);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(collectableCoinObtainedEvent, OnCoinObtained);
        EventManager.Instance.UnsubscribeTo(playerDeathEvent, OnReset);
    }

    void OnCoinObtained(Dictionary<string, object> message)
    {
        _coinsObtained++;

        if (_coinsObtained == _collectableCoins.Length)
        {
            EventManager.Instance.TriggerEvent(allCoinsCollectedEvent, null);
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
