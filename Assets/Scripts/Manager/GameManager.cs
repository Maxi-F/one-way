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
            float prefsSensibility = PlayerPrefs.HasKey("Sensibility")
                ? PlayerPrefs.GetFloat("Sensibility")
                : playerSettingsConfig.sensibility;

            float musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : playerSettingsConfig.musicVolume;
            
            float sfxVolume = PlayerPrefs.HasKey("SoundVolume") ? PlayerPrefs.GetFloat("SoundVolume") : playerSettingsConfig.sfxVolume;

            playerSettingsConfig.sensibility = prefsSensibility;
            playerSettingsConfig.musicVolume = musicVolume;
            playerSettingsConfig.sfxVolume = sfxVolume;
        }
    }
}
