using PlayerScripts;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private string nextLevelName;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private SliderBehaviour pauseSensibilitySlider;
        [SerializeField] private UnityEvent OnDeath;
        
        [Header("Events")]
        [SerializeField] private string playerDeathEvent = "playerDeath";
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
        [SerializeField] private string sensibilityChangedEvent = "sensibilityChanged";

        
        [Header("MenuData")] [SerializeField] private string pauseMenuName = "pause";
        
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        private bool _isPaused = false;
        
        void Start()
        {
            EventManager.Instance.SubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance.SubscribeTo(sensibilityChangedEvent, SetSensibility);
            
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
            EventManager.Instance.UnsubscribeTo(playerDeathEvent, HandleDeath);
            EventManager.Instance.SubscribeTo(sensibilityChangedEvent, SetSensibility);

        }

        public void HandleDeath(Dictionary<string, object> message)
        {
            player.transform.position = _startingPosition;
            
            OnDeath.Invoke();
            player.Stop();
        }

        public void HandleWin()
        {
            _gameplayManager.LevelPassed(nextLevelName);
        }

        public void HandleWinGame()
        {
            _gameplayManager.HandleWin();
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
                
                EventManager.Instance.TriggerEvent(
                    menuDeactivatedEvent,
                    new Dictionary<string, object>() { { "name", pauseMenuName } }
                    );
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _isPaused = false;
            }
            else
            {
                Time.timeScale = 0f;
                
                EventManager.Instance.TriggerEvent(
                    menuActivatedEvent,
                    new Dictionary<string, object>() { { "name", pauseMenuName } }
                );
                
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                _isPaused = true;
            }
        }
    }
}
