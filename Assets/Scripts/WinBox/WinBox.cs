using System.Collections;
using System.Collections.Generic;
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

        if (!isInitiallyActive)
        {
            _guitarRenderer.material = transparentGuitar;
        }

        _isActive = isInitiallyActive;

        EventManager eventManager = FindObjectOfType<EventManager>();
        
        eventManager.SubscribeTo("allCoinsCollected", OnCoinsCollected);
    }

    void OnCoinsCollected(Dictionary<string, object> message)
    {
        _isActive = true;
        
        // TODO Preguntar como mejorar esto mañana
        _guitarRenderer.material = normalGuitar;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(_isActive)
            onWinboxCollided.Invoke();
    }
}
