using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private string menuName;
    [SerializeField] private string menuActivatedEvent = "menuActivated";
    [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
    void Start()
    {
        EventManager.Instance.TriggerEvent(menuActivatedEvent, new Dictionary<string, object>() { { "name", menuName } });
    }

    private void OnDisable()
    {
        EventManager.Instance.TriggerEvent(menuDeactivatedEvent, new Dictionary<string, object>() { { "name", menuName } });
    }
}
