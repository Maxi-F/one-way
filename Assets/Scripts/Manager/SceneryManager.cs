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
        Exit
    }
    
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private SceneNames _initScene;

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
                    UnloadScene(SceneNames.Menu);
                    AddScene(SceneNames.Gameplay);
                    break;  
                case SceneNames.Credits:
                    UnloadScene(SceneNames.Menu);
                    AddScene(SceneNames.Credits);
                    break;
                case SceneNames.Menu:
                    UnloadScene(SceneNames.Credits);
                    UnloadScene(SceneNames.Gameplay);
                    AddScene(SceneNames.Menu);
                    break;
                case SceneNames.Exit:
                    Application.Quit();
                    break;
            }
        }

        private void UnloadScene(SceneNames sceneName)
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
        private void AddScene(SceneNames sceneName)
        {
            SceneManager.LoadScene((int) sceneName, LoadSceneMode.Additive);
            _activeScenes.Add(sceneName);
        }
    }
}
