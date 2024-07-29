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
        
        private Player _player;
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        private bool _isPaused = false;
        private bool _alreadyWon = false;
        
        void Start()
        {
            _player ??= FindObjectOfType<Player>();
            
            EventManager.Instance?.SubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance?.SubscribeTo(enemyHitEvent, HandleEnemyHit);
            EventManager.Instance?.SubscribeTo(sensibilityChangedEvent, SetSensibility);
            EventManager.Instance?.TriggerEvent(initPlayerLivesEvent, new Dictionary<string, object>() { { "value", _player.Lives } });
            
            _startingPosition = _player.transform.position;
            _gameplayManager = FindObjectOfType<GameplayManager>();

            if (_gameplayManager == null)
            {
                Debug.LogWarning("Gameplaymanager not found. Sensibility not setted.");
            }
            else
            {
                _player.Sensibility = _gameplayManager.GetSensibility();   
            }
            
            
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
            
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance?.UnsubscribeTo(sensibilityChangedEvent, SetSensibility); 
            EventManager.Instance?.UnsubscribeTo(enemyHitEvent, HandleEnemyHit);
        }

        public void HandleEnemyHit(Dictionary<string, object> message)
        {
            this.LoseLive(true);
        }

        private void LoseLive(bool fromEnemy)
        {
            _player.LoseLive(fromEnemy);
            
            if (_player.Lives == 0)
            {
                HandleLose();
            }
        }
        
        public void HandleDeath(Dictionary<string, object> message)
        {
            this.LoseLive(false);
            
            _player.transform.position = _startingPosition;
            
            OnDeath.Invoke();
            _player.Stop();
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
            if (_alreadyWon) return;
            
            _gameplayManager.HandleWin();
            EventManager.Instance?.TriggerEvent(levelPassed, null);
            _alreadyWon = true;
        }

        public void BackToMenu()
        {
            _gameplayManager.BackToMenu();
        }

        public void SetSensibility(Dictionary<string, object> message)
        {
            float newSensibility = (float)message["value"];
            
            _player.Sensibility = newSensibility;
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
