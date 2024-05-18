using PlayerScripts;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SceneNames nextLevel;
        
        private Vector3 _startingPosition;
        private GameplayManager _gameplayManager;
        
        void Start()
        {
            _startingPosition = player.transform.position;
            _gameplayManager = FindObjectOfType<GameplayManager>();
        }
    
        public void HandleDeath()
        {
            Debug.Log("dead!");

            player.transform.position = _startingPosition;
            player.Stop();
        }

        public void HandleWin()
        {
            _gameplayManager.LevelPassed(nextLevel);
        }
    }
}
