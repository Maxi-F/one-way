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
    
    [Header("Events")] [SerializeField] private UnityEvent onWinboxCollided;

    private bool _isActive;
    private Renderer _guitarRenderer;
    
    void Start()
    {
        _guitarRenderer = gameObject.transform.GetChild(0).GetComponent<Renderer>();

        SetRenderedMaterial();

        EventManager eventManager = FindObjectOfType<EventManager>();
        
        eventManager.SubscribeTo("allCoinsCollected", OnCoinsCollected);
        eventManager.SubscribeTo("playerDeath", OnReset);
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
        
        // TODO Preguntar como mejorar esto mañana
        _guitarRenderer.material = normalGuitar;
    }

    void OnReset(Dictionary<string, object> message)
    {
        SetRenderedMaterial();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(_isActive)
            onWinboxCollided.Invoke();
    }
}
