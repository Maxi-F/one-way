using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    [Serializable]
    public class MenuData
    {
        public string name;
        public GameObject menuObject;
    }

    public class MenusManager : MonoBehaviour
    {
        [SerializeField] private MenuData[] menus;
        
        [Header("Events")]
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
        private void Start()
        {
            EventManager.Instance?.SubscribeTo(menuActivatedEvent, OnActivateMenu);
            EventManager.Instance?.SubscribeTo(menuDeactivatedEvent, OnDeactivateMenu);

        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(menuActivatedEvent, OnActivateMenu);
            EventManager.Instance?.UnsubscribeTo(menuDeactivatedEvent, OnDeactivateMenu);

        }

        /// <summary>
        /// Event that activates the menu passed from the message.
        /// </summary>
        /// <param name="message">dictionary with a key "name", and a value of a string (Name of the menu to activate).</param>
        private void OnActivateMenu(Dictionary<string, object> message)
        {
            TriggerMenu((string)message["name"], true);
        }
    
        /// <summary>
        /// Event that deactivates the menu passed from the message.
        /// </summary>
        /// <param name="message">dictionary with a key "name", and a value of a string (Name of the menu to deactivate).</param>
        private void OnDeactivateMenu(Dictionary<string, object> message)
        {
            TriggerMenu((string)message["name"], false);
        }

        /// <summary>
        /// Triggers the menu active value, depending on trigger bool passed.
        /// </summary>
        /// <param name="menuName">name of the menu to trigger</param>
        /// <param name="triggerValue">trigger boolean to activate or deactivate.</param>
        private void TriggerMenu(string menuName, bool triggerValue)
        {
            MenuData menu = Array.Find(menus, aMenu => aMenu.name == menuName);

            if (menu == null)
            {
                Debug.LogError($"Menu ${menuName} not found!");
                return;
            }
        
            menu.menuObject?.SetActive(triggerValue);
        }
    }
}