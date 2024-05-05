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
        Gameplay
    }
    
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private SceneNames _initScene;
        [SerializeField] private UnityEvent _onGameplayLoaded;
        
        public void InitScenes()
        {
            LoadScene(_initScene);
        }

        public void LoadScene(SceneNames name)
        {
            switch (name)
            {
                case SceneNames.Boot:
                    break;
                case SceneNames.Gameplay:
                    SceneManager.LoadScene((int) SceneNames.Gameplay, LoadSceneMode.Additive);
                    _onGameplayLoaded.Invoke();
                    break;  
            }
        }
    }
}
