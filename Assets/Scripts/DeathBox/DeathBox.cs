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
    [SerializeField] private UnityEvent onHandleDeath;
    
    private void Start()
    {
        _plane = new Plane(transform.up, transform.position);
    }

    private void Update()
    {
        if (!_plane.GetSide(player.transform.position))
        {
            onHandleDeath.Invoke();
        }
    }
}
