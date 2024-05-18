using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Manager
{
    public enum SceneNames
    {
        Boot,
        Gameplay,
        Menu,
        Credits,
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelFour,
        Exit
    }
    
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private SceneNames _initScene;

        public Action OnCreditsAdded;

        public Action OnGameplayAdded;

        public Action OnMenuAdded;
        
        private readonly List<SceneNames> _activeScenes = new List<SceneNames>();
        
        public void InitScenes()
        {
            LoadScene(_initScene);
            _activeScenes.Add(_initScene);
        }

        public void LoadScene(SceneNames sceneName)
        {
            switch (sceneName)
            {
                case SceneNames.Boot:
                    break;
                case SceneNames.Gameplay:
                    OnGameplayAdded?.Invoke();
                    AddScene(SceneNames.Gameplay);
                    break;  
                case SceneNames.Credits:
                    OnCreditsAdded?.Invoke();
                    Debug.Log(OnCreditsAdded?.GetInvocationList()?.Length);
                    AddScene(SceneNames.Credits);
                    break;
                case SceneNames.Menu:
                    OnMenuAdded?.Invoke();
                    AddScene(SceneNames.Menu);
                    break;
                case SceneNames.Exit:
                    Application.Quit();
                    break;
            }
        }

        public void UnloadScene(SceneNames sceneName)
        {
            if (_activeScenes.Exists(scene => scene == sceneName))
            {
                SceneManager.UnloadSceneAsync((int) sceneName);
            }
            else
            {
                Debug.LogWarning($"{sceneName} not active!");
            }
        }
        public void AddScene(SceneNames sceneName)
        {
            SceneManager.LoadScene((int) sceneName, LoadSceneMode.Additive);
            _activeScenes.Add(sceneName);
        }
    }
}
