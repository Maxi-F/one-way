using PlayerScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Coins.JumpCoin;
using Manager;
using UnityEngine;

public class JumpCoinManager : MonoBehaviour
{
    [SerializeField] private float secondsUntilEnableCoin = 4;
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

    private void OnCoinObtained(Dictionary<string, object> message)
    {
        GameObject aGameObject = (GameObject)message["gameObject"];

        EventManager.Instance?.TriggerEvent(modifyJumpValuesEvent, new Dictionary<string, object>() { {"value", 1} });
        
        StartCoroutine(EnableCoin(aGameObject));
    }

    IEnumerator EnableCoin(GameObject coinObject)
    {
        yield return new WaitForSeconds(secondsUntilEnableCoin);
        
        coinObject.GetComponent<JumpCoin>().Enable();
    }
}
