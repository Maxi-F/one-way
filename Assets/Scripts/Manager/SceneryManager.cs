using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private string _initSceneName;
        [SerializeField] private ScenesData _scenesData;
        
        private readonly List<SerializableScene> _activeScenes = new List<SerializableScene>();
        
        public void InitScenes()
        {
            LoadScene(_initSceneName);
        }

        public void LoadScene(string sceneName)
        {
            if (sceneName == "Exit")
            {
                Debug.Log("Quitting...");
                Application.Quit();
                return;
            }
            
            SerializableScene scene = _scenesData.GetSceneByName(sceneName);
            
            scene.OnSceneAdded?.Invoke();
            AddScene(scene);
        }

        public void UnloadScene(string aSceneName)
        {
            SerializableScene aScene = _scenesData.GetSceneByName(aSceneName);
            
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
            SerializableScene aScene = _scenesData.GetSceneByName(sceneName);

            aScene.OnSceneAdded += action;
        }
        
        public void UnsubscribeEventToAddScene(string sceneName, Action action)
        {
            SerializableScene aScene = _scenesData.GetSceneByName(sceneName);

            aScene.OnSceneAdded -= action;
        }
    }
}
