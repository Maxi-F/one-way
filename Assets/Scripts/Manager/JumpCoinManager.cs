using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCoinManager : MonoBehaviour
{
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
        Debug.Log("obtained coin!");   
    }
}
