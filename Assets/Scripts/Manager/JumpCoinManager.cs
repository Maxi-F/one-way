using PlayerScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class JumpCoinManager : MonoBehaviour
{
    [SerializeField] private float secondsUntilEnableCoin = 4;
    [SerializeField] private string coinObtainedEvent = "coinObtained";
    
    private void Start()
    {
        EventManager.Instance.SubscribeTo(coinObtainedEvent, OnCoinObtained);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(coinObtainedEvent, OnCoinObtained);
    }

    private void OnCoinObtained(Dictionary<string, object> message)
    {
        GameObject aGameObject = (GameObject)message["gameObject"];

        StartCoroutine(EnableCoin(aGameObject));
    }

    IEnumerator EnableCoin(GameObject coinObject)
    {
        yield return new WaitForSeconds(secondsUntilEnableCoin);
        
        coinObject.SetActive(true);
    }
}
