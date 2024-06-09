using ScriptableObjects;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager _sceneryManager;
        [SerializeField] private SensibilitySettings sensibilitySettings;
        private void Awake()
        {
            _sceneryManager.InitScenes();
            float prefsSensibility = PlayerPrefs.GetFloat("Sensibility");

            sensibilitySettings.sensibility = prefsSensibility;
        }
    }
}
