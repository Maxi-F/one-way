using PlayerScripts;
using System;
using System.Collections.Generic;
using Audio;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private string nextLevelName;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private SensibilitySlider pauseSensibilitySensibilitySlider;
        [SerializeField] private UnityEvent OnDeath;
        
        [Header("Events")]
        [SerializeField] private string playerDeathEvent = "playerDeath";
        [SerializeField] private string enemyHitEvent = "enemyHit";
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
        [SerializeField] private string sensibilityChangedEvent = "sensibilityChanged";
        [SerializeField] private string initPlayerLivesEvent = "initPlayerLives";
        [SerializeField] private string levelPassed = "levelPassed";
        
        [Header("MenuData")] 
        [SerializeField] private string pauseMenuName = "pause";
        
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        private bool _isPaused = false;
        
        void Start()
        {
            EventManager.Instance?.SubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance?.SubscribeTo(enemyHitEvent, HandleEnemyHit);
            EventManager.Instance?.SubscribeTo(sensibilityChangedEvent, SetSensibility);
            EventManager.Instance?.TriggerEvent(initPlayerLivesEvent, new Dictionary<string, object>() { { "value", player.Lives } });
            
            _startingPosition = player.transform.position;
            _gameplayManager = FindObjectOfType<GameplayManager>();

            if (_gameplayManager == null)
            {
                Debug.LogWarning("Gameplaymanager not found. Sensibility not setted.");
            }
            else
            {
                player.Sensibility = _gameplayManager.GetSensibility();   
            }
            
            
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
            
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance?.SubscribeTo(sensibilityChangedEvent, SetSensibility);
        }

        public void HandleEnemyHit(Dictionary<string, object> message)
        {
            this.LoseLive(true);
        }

        private void LoseLive(bool fromEnemy)
        {
            player.LoseLive(fromEnemy);
            
            if (player.Lives == 0)
            {
                HandleLose();
                
                return;
            }
        }
        
        public void HandleDeath(Dictionary<string, object> message)
        {
            this.LoseLive(false);
            
            player.transform.position = _startingPosition;
            
            OnDeath.Invoke();
            player.Stop();
        }

        public void HandleLose()
        {
            _gameplayManager.HandleLose();
        }
        
        public void HandleWin()
        {
            _gameplayManager.LevelPassed(nextLevelName);
            EventManager.Instance?.TriggerEvent(levelPassed, null);
        }

        public void HandleWinGame()
        {
            _gameplayManager.HandleWin();
            EventManager.Instance?.TriggerEvent(levelPassed, null);
        }

        public void BackToMenu()
        {
            _gameplayManager.BackToMenu();
        }

        public void SetSensibility(Dictionary<string, object> message)
        {
            float newSensibility = (float)message["value"];
            
            player.Sensibility = newSensibility;
            _gameplayManager.SetSensibility(newSensibility);
        }

        public void TogglePause()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                
                EventManager.Instance?.TriggerEvent(
                    menuDeactivatedEvent,
                    new Dictionary<string, object>() { { "name", pauseMenuName } }
                    );
                
                AudioManager.Instance?.ResumeAll();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _isPaused = false;
            }
            else
            {
                Time.timeScale = 0f;
                
                EventManager.Instance?.TriggerEvent(
                    menuActivatedEvent,
                    new Dictionary<string, object>() { { "name", pauseMenuName } }
                );
                
                AudioManager.Instance?.PauseAllSounds();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                _isPaused = true;
            }
        }
    }
}
