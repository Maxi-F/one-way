using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

[Serializable]
public class MenuData
{
    public string name;
    public GameObject menuObject;
}

public class MenusManager : MonoBehaviour
{
    [SerializeField] private MenuData[] menus;
    [SerializeField] private string menuActivatedEvent = "menuActivated";
    [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
    private void Start()
    {
        EventManager.Instance.SubscribeTo(menuActivatedEvent, ActivateMenu);
        EventManager.Instance.SubscribeTo(menuDeactivatedEvent, DeactivateMenu);

    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(menuActivatedEvent, ActivateMenu);
        EventManager.Instance.UnsubscribeTo(menuDeactivatedEvent, DeactivateMenu);

    }

    private void ActivateMenu(Dictionary<string, object> message)
    {
        TriggerMenu((string)message["name"], true);
    }
    
    private void DeactivateMenu(Dictionary<string, object> message)
    {
       TriggerMenu((string)message["name"], false);
    }

    private void TriggerMenu(string name, bool triggerValue)
    {
        MenuData menu = Array.Find(menus, aMenu => aMenu.name == name);

        if (menu == null)
        {
            Debug.LogError($"Menu ${name} not found!");
            return;
        }
        
        menu.menuObject.SetActive(triggerValue);
    }
}
