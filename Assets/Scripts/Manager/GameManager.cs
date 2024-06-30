using ScriptableObjects;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager _sceneryManager;
        [FormerlySerializedAs("playerSettings")] [FormerlySerializedAs("sensibilitySettings")] [SerializeField] private PlayerSettingsConfig playerSettingsConfig;
        private void Awake()
        {
            _sceneryManager.InitScenes();
            float prefsSensibility = PlayerPrefs.GetFloat("Sensibility");

            playerSettingsConfig.sensibility = prefsSensibility;
        }
    }
}
