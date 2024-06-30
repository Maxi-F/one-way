using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private string[] scenesToSubscribeTo;
        [Tooltip("Current scene name")] [SerializeField] private string sceneName;
        [Tooltip("Optional scenes to activate with the current scene")] [SerializeField] private string[] optionalScenes = new string[] {};
        
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

        /// <summary>
        /// Subscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void SubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager.SubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        /// <summary>
        /// Unsubscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager.UnsubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        /// <summary>
        /// Unloads current scene and optional scenes.
        /// </summary>
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
