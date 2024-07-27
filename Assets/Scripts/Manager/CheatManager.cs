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
        
        [Header("Events")]
        [SerializeField] private UnityEvent OnFlyToggled;
        [SerializeField] private string onFlashToggle = "onFlashToggle";

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

        public void ToggleGodmode()
        {
            player.IsInGodMode = !player.IsInGodMode;
        }

        public void ToggleFlash()
        {
            EventManager.Instance?.TriggerEvent(onFlashToggle, null);
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
