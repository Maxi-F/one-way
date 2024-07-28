using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace UI
{
    public class ToggleListener : MonoBehaviour
    {
        [SerializeField] private string eventToTrigger;

        public void HandleToggle(bool value)
        {
            EventManager.Instance?.TriggerEvent(eventToTrigger, new Dictionary<string, object>() { { "value", value } });
        }
    }
}
