using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Manager
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private string initSceneName = "Menu";
        [SerializeField] private ScenesData scenesData;
        [SerializeField] private string allMenusScene;
        
        private readonly List<SerializableScene> _activeScenes = new List<SerializableScene>();
        
        public void InitScenes()
        {
            LoadScene(allMenusScene);
            LoadScene(initSceneName);
        }

        public void LoadScene(string sceneName)
        {
            if (sceneName == "Exit")
            {
                Debug.Log("Quitting...");
                Application.Quit();
                return;
            }
            
            SerializableScene scene = scenesData.GetSceneByName(sceneName);
            
            scene.OnSceneAdded?.Invoke();
            AddScene(scene);
        }

        public void UnloadScene(string aSceneName)
        {
            SerializableScene aScene = scenesData.GetSceneByName(aSceneName);
            
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
        public void AddScene(SerializableScene scene)
        {
            SceneManager.LoadScene(scene.index, LoadSceneMode.Additive);
            _activeScenes.Add(scene);
        }

        public void SubscribeEventToAddScene(string sceneName, Action action)
        {
            SerializableScene aScene = scenesData.GetSceneByName(sceneName);

            aScene.OnSceneAdded += action;
        }
        
        public void UnsubscribeEventToAddScene(string sceneName, Action action)
        {
            SerializableScene aScene = scenesData.GetSceneByName(sceneName);

            aScene.OnSceneAdded -= action;
        }
    }
}
