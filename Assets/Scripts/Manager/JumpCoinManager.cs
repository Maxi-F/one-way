using System.Collections;
using System.Collections.Generic;
using Coins.JumpCoin;
using UnityEngine;

namespace Manager
{
    public class JumpCoinManager : MonoBehaviour
    {
        [SerializeField] private float secondsUntilEnableCoin = 4;
        
        [Header("Events")]
        [SerializeField] private string coinObtainedEvent = "coinObtained";
        [SerializeField] private string modifyJumpValuesEvent = "noteModified";

        private void Start()
        {
            EventManager.Instance?.SubscribeTo(coinObtainedEvent, OnCoinObtained);
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(coinObtainedEvent, OnCoinObtained);
        }

        /// <summary>
        /// Event that executes when coin is obtained.
        /// </summary>
        private void OnCoinObtained(Dictionary<string, object> message)
        {
            GameObject aGameObject = (GameObject)message["gameObject"];

            EventManager.Instance?.TriggerEvent(modifyJumpValuesEvent, new Dictionary<string, object>() { {"value", 1} });
        
            StartCoroutine(EnableCoin(aGameObject));
        }

        /// <summary>
        /// Coroutine that enables the coin after some time.
        /// </summary>
        /// <param name="coinObject">jump note to enable again.</param>
        IEnumerator EnableCoin(GameObject coinObject)
        {
            yield return new WaitForSeconds(secondsUntilEnableCoin);
        
            coinObject.GetComponent<JumpCoin>().Enable();
        }
    }
}
