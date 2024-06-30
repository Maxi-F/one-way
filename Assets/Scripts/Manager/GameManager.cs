using ScriptableObjects;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager sceneryManager;
        [SerializeField] private PlayerSettingsConfig playerSettingsConfig;
        private void Awake()
        {
            sceneryManager.InitScenes();
            float prefsSensibility = PlayerPrefs.GetFloat("Sensibility");

            playerSettingsConfig.sensibility = prefsSensibility;
        }
    }
}
