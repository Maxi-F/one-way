using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableObjects.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpCoin : MonoBehaviour
{
    [Header("Coin config")]
    [SerializeField] private JumpCoinConfig config;

    [Header("Hover settings")] 
    [SerializeField] private float hoverVelocity = 5f;
    [SerializeField] private float hoverDistance = 0.2f;
    
    private Vector3 _center;
    private GameObject _instantiatedNote;
    private int _actualDirection = 1;
    
    
    public void Start()
    {
        JumpCoinFactory factory = new JumpCoinFactory(config);

        _instantiatedNote = factory.CreateJumpCoin(gameObject);

        _center = _instantiatedNote.transform.position;
    }

    public void Update()
    {
        _actualDirection = (_instantiatedNote.transform.position - _center).magnitude > hoverDistance ? -_actualDirection : _actualDirection;

        _instantiatedNote.transform.position += new Vector3(0,  _actualDirection * hoverVelocity * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddJump();
            Destroy(gameObject);
        }
    }
}
