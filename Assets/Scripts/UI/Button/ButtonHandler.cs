using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Button
{
    public class ButtonHandler : MonoBehaviour
    {
        private SceneryManager _sceneryManager;
        [SerializeField] private bool isExit = false;
        [SerializeField] private SceneChangeData sceneName;
        
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
            
            _sceneryManager.LoadScene(isExit ? "Exit" : sceneName.sceneName);
        }
    }
    
}
