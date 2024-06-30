using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager _sceneryManager;
        [FormerlySerializedAs("sensibilitySettings")] [SerializeField] private PlayerSettings playerSettings;
        private void Awake()
        {
            _sceneryManager.InitScenes();
            float prefsSensibility = PlayerPrefs.GetFloat("Sensibility");

            playerSettings.sensibility = prefsSensibility;
        }
    }
}
