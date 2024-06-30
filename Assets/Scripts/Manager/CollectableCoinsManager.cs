using System.Collections.Generic;
using Audio;
using Coins;
using Coins.CollectableCoin;
using PlayerScripts;
using UnityEngine;

namespace Manager
{
    public class CollectableCoinsManager : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Sounds")] 
        [SerializeField] private string obtainStarSound = "star";
        [SerializeField] private string allStarsCollectedSound = "allStarsCollected";
    
        [Header("Events")]
        [SerializeField] private string collectableCoinObtainedEvent = "collectableCoinObtained";
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
        
            EventManager.Instance?.TriggerEvent(collectableCoinsCountEvent, new Dictionary<string, object>() { { "value", _collectableCoins.Length } });
            EventManager.Instance?.SubscribeTo(collectableCoinObtainedEvent, OnCoinObtained);
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(collectableCoinObtainedEvent, OnCoinObtained);
        }

        /// <summary>
        /// When the event is called, it sums one coin obtained and checks if its the last coin to obtain.
        /// If it is the last one, it triggers the all coins collected event.
        /// </summary>
        void OnCoinObtained(Dictionary<string, object> message)
        {
            _coinsObtained++;

            if (_coinsObtained == _collectableCoins.Length)
            {
                AudioManager.Instance?.PlaySound(allStarsCollectedSound);
                EventManager.Instance?.TriggerEvent(allCoinsCollectedEvent, null);
            }
            else
            {
                AudioManager.Instance?.PlaySound(obtainStarSound);
            }
        }
    }
}
