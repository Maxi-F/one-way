using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

public class DeathBox : MonoBehaviour
{
    private Plane _plane;
    
    [SerializeField] private Player player;
    
    [Header("Events")]
    [SerializeField] private string playerDeathEvent = "playerDeath";
    
    [Header("Sounds")]
    [SerializeField] private string lostLiveSound = "lostLife";
    
    private void Start()
    {
        _plane = new Plane(transform.up, transform.position);
    }

    private void Update()
    {
        if (!_plane.GetSide(player.transform.position))
        {
            AudioManager.Instance?.PlaySound(lostLiveSound);
            EventManager.Instance?.TriggerEvent(playerDeathEvent, null);
        }
    }
}
