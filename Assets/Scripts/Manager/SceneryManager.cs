using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private int[] _scenes;
        [SerializeField] private int _initScene;
        [SerializeField] private UnityEvent _onGameplayLoaded;
        
        public void InitScenes()
        {
            int scene = _scenes[_initScene];

            LoadScene(scene);
        }

        public void LoadScene(int index)
        {
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    SceneManager.LoadScene(index, LoadSceneMode.Additive);
                    _onGameplayLoaded.Invoke();
                    break;  
            }
        }
    }
}
