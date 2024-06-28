using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Button
{
    public class ButtonHandler : MonoBehaviour
    {
        private SceneryManager _sceneryManager;
        [SerializeField] private string sceneName;
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
        private void Start()
        {
            _sceneryManager = FindObjectOfType<SceneryManager>();
        }
        
        public void OnButtonClick()
        {
            if (_sceneryManager == null)
            {
                Debug.LogError($"{nameof(_sceneryManager)} is null!");
                return;
            }
            
            _sceneryManager.LoadScene(sceneName);
        }

        public void Deactivate(string canvasName)
        {
            EventManager.Instance.TriggerEvent(
                menuDeactivatedEvent,
                new Dictionary<string, object>() { { "name", canvasName } }
                );
        }
    }
    
}
