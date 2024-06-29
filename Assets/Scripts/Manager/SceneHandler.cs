using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private string[] scenesToSubscribeTo;
        [SerializeField] private string sceneName;
        [SerializeField] private string[] optionalScenes = new string[] {};
        
        private SceneryManager _sceneryManager;
        
        private void OnEnable()
        {
            _sceneryManager = FindObjectOfType<SceneryManager>();

            foreach (var optionalScene in optionalScenes)
            {
                _sceneryManager.LoadScene(optionalScene);
            }

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
                _sceneryManager.SubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager.UnsubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        private void UnloadScene()
        {
            _sceneryManager.UnloadScene(sceneName);

            foreach (var optionalScene in optionalScenes)
            {
                _sceneryManager.UnloadScene(optionalScene);
            }
        }
    }
}
