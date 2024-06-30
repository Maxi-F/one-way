using System.Collections.Generic;
using Audio;
using Manager;
using UnityEngine;

namespace UI.Button
{
    public class ButtonHandler : MonoBehaviour
    {
        private SceneryManager _sceneryManager;
        [SerializeField] private string sceneName;
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";

        [Header("Sounds")] [SerializeField] private string clickSound = "click";
        
        private void Start()
        {
            _sceneryManager = FindObjectOfType<SceneryManager>();
        }
        
        /// <summary>
        /// Event that handles button click.
        /// </summary>
        public void OnButtonClick()
        {
            if (_sceneryManager == null)
            {
                Debug.LogError($"{nameof(_sceneryManager)} is null!");
                return;
            }
            
            AudioManager.Instance?.PlaySound(clickSound);
            _sceneryManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Deactivates a menu canvas on click.
        /// </summary>
        public void Deactivate(string canvasName)
        {
            EventManager.Instance?.TriggerEvent(
                menuDeactivatedEvent,
                new Dictionary<string, object>() { { "name", canvasName }, { "isDeactivating", true } }
                );
        }
    }
    
}
