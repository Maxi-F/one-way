using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private SceneChangeData[] scenesToSubscribeTo;
        [SerializeField] private SceneChangeData sceneName;

        private SceneryManager _sceneryManager;
        
        private void OnEnable()
        {
            _sceneryManager = FindObjectOfType<SceneryManager>();
            
            SubscribeToActions();
        }

        private void OnDisable()
        {
            UnsubscribeToActions();
        }

        private void SubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager.SubscribeEventToAddScene(aSceneName.sceneName, UnloadScene);
            });
        }
        
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager.UnsubscribeEventToAddScene(aSceneName.sceneName, UnloadScene);
            });
        }
        
        private void UnloadScene()
        {
            _sceneryManager.UnloadScene(sceneName.sceneName);
        }
    }
}
