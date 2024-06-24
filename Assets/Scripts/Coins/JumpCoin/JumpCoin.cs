using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableObjects.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpCoin : WithDebugRemover
{
    [Header("Coin config")]
    [SerializeField] private JumpCoinConfig config;
    
    private Vector3 _center;
    private GameObject _instantiatedNote;
    private EventManager _eventManager;
    private int _actualDirection = 1;
    
    
    public void Start()
    {
        RemoveDebug();
        _eventManager ??= FindObjectOfType<EventManager>();
        
        JumpCoinFactory factory = new JumpCoinFactory(config);

        _instantiatedNote = factory.CreateJumpCoin(gameObject);

        _center = _instantiatedNote.transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddJump();

            _eventManager.TriggerEvent(
                "coinObtained",
                new Dictionary<string, object>()
                {
                    { "gameObject", gameObject }
                });
            
            
            gameObject.SetActive(false);
        }
    }
}
