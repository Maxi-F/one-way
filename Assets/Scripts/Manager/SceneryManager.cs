using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Manager
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private ScenesDataConfig scenesDataConfig;
        [Tooltip("Scenes that are on boot")] [SerializeField] private string[] initScenes = new string[] { "Menu", "AllMenus", "Managers" };
        
        private readonly List<SerializableScene> _activeScenes = new List<SerializableScene>();
        
        /// <summary>
        /// Loads all init scenes.
        /// </summary>
        public void InitScenes()
        {
            foreach (var initScene in initScenes)
            {
                LoadScene(initScene);
            }
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <param name="sceneName">The scene name to load.</param>
        public void LoadScene(string sceneName)
        {
            if (sceneName == "Exit")
            {
                Debug.Log("Quitting...");
                Application.Quit();
                return;
            }
            
            SerializableScene scene = scenesDataConfig.GetSceneByName(sceneName);
            
            scene.OnSceneAdded?.Invoke();
            AddScene(scene);
        }

        /// <summary>
        /// Unloads the scene.
        /// </summary>
        /// <param name="sceneName">The scene name to unload.</param>
        public void UnloadScene(string aSceneName)
        {
            SerializableScene aScene = scenesDataConfig.GetSceneByName(aSceneName);
            
            if (_activeScenes.Exists(scene => scene.name == aScene.name))
            {
                SceneManager.UnloadSceneAsync(aScene.index);
                aScene.OnSceneRemoved?.Invoke();
                _activeScenes.RemoveAt(_activeScenes.FindIndex(scene => scene.name == aScene.name));
            }
            else
            {
                Debug.LogWarning($"{aScene.name} not active!");
            }
        }

        /// <summary>
        /// Loads a new scene with scene mode aditive.
        /// </summary>
        /// <param name="scene">Serializable scene to load.</param>
        private void AddScene(SerializableScene scene)
        {
            SceneManager.LoadScene(scene.index, LoadSceneMode.Additive);
            _activeScenes.Add(scene);
        }

        /// <summary>
        /// Subscribes an event to an Add scene event
        /// </summary>
        /// <param name="sceneName">Scene name to load the event to</param>
        /// <param name="action">Action to subscribe.</param>
        public void SubscribeEventToAddScene(string sceneName, Action action)
        {
            SerializableScene aScene = scenesDataConfig.GetSceneByName(sceneName);

            aScene.OnSceneAdded += action;
        }
        
        /// <summary>
        /// Unsubscribes an event from the Add scene event
        /// </summary>
        /// <param name="sceneName">Scene name to unload the action</param>
        /// <param name="action">Action to unsubscribe.</param>
        public void UnsubscribeEventToAddScene(string sceneName, Action action)
        {
            SerializableScene aScene = scenesDataConfig.GetSceneByName(sceneName);

            aScene.OnSceneAdded -= action;
        }
    }
}
