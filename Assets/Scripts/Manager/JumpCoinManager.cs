using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCoinManager : MonoBehaviour
{
    [SerializeField] private float secondsUntilEnableCoin = 4;
    
    private JumpCoin[] _jumpCoins;
    private EventManager _eventManager;
    
    private void Start()
    {
        _jumpCoins ??= GetComponents<JumpCoin>();
        _eventManager ??= FindObjectOfType<EventManager>();
        
        _eventManager.SubscribeTo("coinObtained", OnCoinObtained);
    }

    private void OnDisable()
    {
        _eventManager.UnsubscribeTo("coinObtained", OnCoinObtained);
    }

    public void OnCoinObtained(Dictionary<string, object> message)
    {
        GameObject gameObject = (GameObject)message["gameObject"];

        StartCoroutine(EnableCoin(gameObject));
    }

    IEnumerator EnableCoin(GameObject coinObject)
    {
        yield return new WaitForSeconds(secondsUntilEnableCoin);
        
        coinObject.SetActive(true);
    }
}
