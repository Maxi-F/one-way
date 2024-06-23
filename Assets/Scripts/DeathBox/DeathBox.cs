using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

public class DeathBox : MonoBehaviour
{
    private Plane _plane;
    
    [SerializeField] Player player;
    
    private EventManager _eventManager;    
    private void Start()
    {
        _plane = new Plane(transform.up, transform.position);
        _eventManager = FindObjectOfType<EventManager>();

        if (_eventManager == null)
        {
            Debug.LogError("Event manager is null");
        } 
    }

    private void Update()
    {
        if (!_plane.GetSide(player.transform.position))
        {
            Debug.Log("triggerring...");
            _eventManager.TriggerEvent("playerDeath", null);
        }
    }
}
