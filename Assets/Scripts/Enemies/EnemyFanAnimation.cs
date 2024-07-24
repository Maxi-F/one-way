using UnityEngine;

namespace Enemies
{
    public class EnemyFanAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void ResetAnimation()
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isDead", false);
        }
        
        /// <summary>
        /// Handles enemy fan running or idle animation
        /// </summary>
        public void HandleRunning(bool isRunning)
        {
            animator.SetBool("isRunning", isRunning);
        }

        public void HandleDead(bool isDead)
        {
            animator.SetBool("isDead", isDead);
        }
    }
}
