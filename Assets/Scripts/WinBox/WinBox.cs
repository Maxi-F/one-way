using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;

public class WinBox : MonoBehaviour
{
    [SerializeField] private Material transparentGuitar;
    [SerializeField] private Material normalGuitar;
    [SerializeField] private bool isInitiallyActive = true;
    
    [Header("Events")] 
    [SerializeField] private UnityEvent onWinboxCollided;

    [Header("Event names")]
    [SerializeField] private string allCoinsCollectedEvent = "allCoinsCollected";
    
    private bool _isActive;
    private Renderer _guitarRenderer;
    
    void Start()
    {
        _guitarRenderer = gameObject.transform.GetChild(0).GetComponent<Renderer>();

        SetRenderedMaterial();
        
        EventManager.Instance.SubscribeTo(allCoinsCollectedEvent, OnCoinsCollected); 
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(allCoinsCollectedEvent, OnCoinsCollected);
    }

    void SetRenderedMaterial()
    {
        _isActive = isInitiallyActive;
        
        if (!_isActive)
        {
            _guitarRenderer.material = transparentGuitar;
        }
    }
    
    void OnCoinsCollected(Dictionary<string, object> message)
    {
        _isActive = true;
        
        _guitarRenderer.material = normalGuitar;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(_isActive)
            onWinboxCollided.Invoke();
    }
}
