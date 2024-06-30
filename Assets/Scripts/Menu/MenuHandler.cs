using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Menu
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] private string menuName;
    
        [Header("Events")]
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
        void Start()
        {
            EventManager.Instance?.TriggerEvent(menuActivatedEvent, new Dictionary<string, object>() { { "name", menuName } });
        }

        private void OnDisable()
        {
            EventManager.Instance?.TriggerEvent(menuDeactivatedEvent, new Dictionary<string, object>() { { "name", menuName } });
        }
    }
}
