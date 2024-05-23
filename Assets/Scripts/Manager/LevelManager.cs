using PlayerScripts;
using System;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SceneNames nextLevel;
        [SerializeField] private GameObject pauseCanvas;
        
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        private bool _isPaused = false;
        
        void Start()
        {
            _startingPosition = player.transform.position;
            _gameplayManager = FindObjectOfType<GameplayManager>();
            player.Sensibility = _gameplayManager.Sensibility;
        }
    
        public void HandleDeath()
        {
            player.transform.position = _startingPosition;
            player.Stop();
        }

        public void HandleWin()
        {
            Debug.Log("WHY");
            _gameplayManager.LevelPassed(nextLevel);
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
            _gameplayManager.Sensibility = newSensibility;
        }

        public void TogglePause()
        {
            Time.timeScale = _isPaused ? 1f : 0f;
            pauseCanvas.SetActive(!_isPaused);
            _isPaused = !_isPaused;
        }
    }
}
