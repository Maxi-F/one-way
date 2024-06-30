using System.Collections.Generic;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameplayManager : MonoBehaviour
    {
        private string _activeLevelSceneName;
        private SceneryManager _sceneryManager;
    
        [SerializeField] private PlayerSettingsConfig playerSettingsConfig;
        
        [Header("SceneNames")]
        [SerializeField] private string initLevelSceneName = "Level1";
        [SerializeField] private string menuSceneName = "Menu";
        [SerializeField] private string gameplaySceneName = "Gameplay";
        [SerializeField] private string gameplayUIMenuName = "gameplayUI";

    
        [Header("Events")]
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
    
        void Awake()
        {
            _activeLevelSceneName = initLevelSceneName;
            _sceneryManager ??= FindObjectOfType<SceneryManager>();
            _sceneryManager.LoadScene(_activeLevelSceneName);
            _sceneryManager.SubscribeEventToAddScene(menuSceneName, UnloadGameplay);
        
            EventManager.Instance?.TriggerEvent(menuActivatedEvent, new Dictionary<string, object>() { { "name", gameplayUIMenuName } });
        }

        private void OnDisable()
        {
            EventManager.Instance?.TriggerEvent(menuDeactivatedEvent, new Dictionary<string, object>() { { "name", gameplayUIMenuName } });
        }

        /// <summary>
        /// Changes the level scene, unloading current scene and
        /// loading the next scene passed as a parameter
        /// </summary>
        /// <param name="nextLevel">The next level scene.</param>
        public void LevelPassed(string nextLevel)
        {
            if (_activeLevelSceneName == nextLevel) return;
            _sceneryManager.UnloadScene(_activeLevelSceneName);
            _sceneryManager.LoadScene(nextLevel);
            _activeLevelSceneName = nextLevel;
        }

        /// <summary>
        /// Handles win behaviour.
        /// </summary>
        public void HandleWin()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        
            // TODO win scene
            _sceneryManager.LoadScene(menuSceneName);
        }

        /// <summary>
        /// Unloads the gameplay scenes.
        /// </summary>
        private void UnloadGameplay()
        {
            _sceneryManager.UnloadScene(_activeLevelSceneName);
            _sceneryManager.UnloadScene(gameplaySceneName);
            _sceneryManager.UnsubscribeEventToAddScene(menuSceneName, UnloadGameplay);
        }

        /// <summary>
        /// Loads the menu scene.
        /// </summary>
        public void BackToMenu()
        {
            _sceneryManager.LoadScene(menuSceneName);
        }

        /// <summary>
        /// Sets the new sensitivity on player prefs and player SO.
        /// </summary>
        public void SetSensibility(float newSensibility)
        {
            playerSettingsConfig.sensibility = newSensibility;
            PlayerPrefs.SetFloat("Sensibility", playerSettingsConfig.sensibility);
        }

        /// <summary>
        /// Sets the new music volume on player prefs and player SO.
        /// </summary>
        public void SetMusicVolume(float newMusicVolume)
        {
            playerSettingsConfig.musicVolume = newMusicVolume;
            PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
        }

        /// <summary>
        /// Get current music volume.
        /// </summary>
        /// <returns>Current music volume.</returns>
        public float GetMusicVolume()
        {
            return playerSettingsConfig.musicVolume;
        }
    
        /// <summary>
        /// Sets the new sound volume on player prefs and player SO.
        /// </summary>
        public void SetSoundVolume(float newMusicVolume)
        {
            playerSettingsConfig.sfxVolume = newMusicVolume;
            PlayerPrefs.SetFloat("SoundVolume", newMusicVolume);
        }

        /// <summary>
        /// Get current SFX volume.
        /// </summary>
        /// <returns>Current SFX volume.</returns>
        public float GetSoundVolume()
        {
            return playerSettingsConfig.sfxVolume;
        }

        /// <summary>
        /// Get current Sensibility.
        /// </summary>
        /// <returns>Current Sensibility.</returns>
        public float GetSensibility()
        {
            return playerSettingsConfig.sensibility;
        }

        /// <summary>
        /// Handles lose behaviour.
        /// </summary>
        public void HandleLose()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
     
            // TODO lose scene
            _sceneryManager.LoadScene(menuSceneName);
        }
    }
}
