using PlayerScripts;
using System;
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
        
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        private bool _isPaused = false;
        
        void Start()
        {
            _startingPosition = player.transform.position;
            _gameplayManager = FindObjectOfType<GameplayManager>();
            player.Sensibility = _gameplayManager.GetSensibility();
            pauseSensibilitySlider ??= GetComponent<SliderBehaviour>();
            
            pauseSensibilitySlider.UseValue(player.Sensibility);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
        }
    
        public void HandleDeath()
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

        public void SetSensibility(float newSensibility)
        {
            player.Sensibility = newSensibility;
            _gameplayManager.SetSensibility(newSensibility);
        }

        public void TogglePause()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                pauseCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _isPaused = false;
            }
            else
            {
                Time.timeScale = 0f;
                pauseCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                _isPaused = true;
            }
        }
    }
}
