using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private SceneNames[] scenesToSubscribeTo;
        [SerializeField] private SceneNames scene;

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
            Array.ForEach(scenesToSubscribeTo, (action) =>
            {
                switch (action)
                {
                    case SceneNames.Gameplay:
                        _sceneryManager.OnGameplayAdded += UnloadScene;
                        break;
                    case SceneNames.Menu:
                        _sceneryManager.OnMenuAdded += UnloadScene;
                        break;
                    case SceneNames.Credits:
                        _sceneryManager.OnCreditsAdded += UnloadScene;
                        break;
                }
            });
        }
        
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (action) =>
            {
                switch (action)
                {
                    case SceneNames.Gameplay:
                        _sceneryManager.OnGameplayAdded -= UnloadScene;
                        break;
                    case SceneNames.Menu:
                        _sceneryManager.OnMenuAdded -= UnloadScene;
                        break;
                    case SceneNames.Credits:
                        _sceneryManager.OnCreditsAdded -= UnloadScene;
                        break;
                }
            });
        }
        
        private void UnloadScene()
        {
            _sceneryManager.UnloadScene(scene);
        }
    }
}
