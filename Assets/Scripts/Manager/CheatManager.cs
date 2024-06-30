using PlayerScripts;
using PlayerScripts.Behaviours;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Manager
{
    public class CheatManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private Player player;
        [SerializeField] private FlyBehaviour flyBehaviour;
        [SerializeField] private WalkingBehaviour walkingBehaviour;
        [SerializeField] private bool isLastLevel;
    
        [SerializeField] private UnityEvent OnFlyToggled;
    
        /// <summary>
        /// Toggles fly on player.
        /// </summary>
        public void ToggleFly()
        {
            if (player.IsFlying)
            {
                player.TouchesGround();
                player.IsFlying = false;
            }
            else
            {
            
                player.Fly();
                player.IsFlying = true;
            }
        
            OnFlyToggled?.Invoke();
        }

        /// <summary>
        /// Passes to the next level. If it is on last level, it wins the game.
        /// </summary>
        public void PassLevel()
        {
            if (isLastLevel)
            {
                levelManager.HandleWinGame();
            }
            else
            {
                levelManager.HandleWin();
            }
        }
    }
}
