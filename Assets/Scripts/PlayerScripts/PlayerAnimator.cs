using PlayerScripts.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private MoveController _moveController;
        private Player _player;

        [Header("Animation Parameters")]
        [SerializeField] private string walkingSpeed = "walkingSpeed";
        [SerializeField] private string isAttacking = "isAttacking";
        [SerializeField] private string isJumping = "isJumping";
        [SerializeField] private string isFalling = "isFalling";
        [SerializeField] private string isHanging = "isHanging";
        [SerializeField] private string isDoubleJumping = "isDoubleJumping";

        public void Start()
        {
            _player ??= GetComponent<Player>();
            _moveController ??= GetComponent<MoveController>();
            
            if (_player == null)
            {
                Debug.LogError($"{nameof(_player)} is null! disabling {nameof(PlayerAnimator)}");
                enabled = false;
            }

            if (_moveController == null)
            {
                Debug.LogError($"{nameof(_moveController)} is null! disabling {nameof(PlayerAnimator)}");
                enabled = false;
            }
        }
        
        /// <summary>
        /// Handles walk flags on animator.
        /// </summary>
        public void HandleWalk()
        {
            animator.SetFloat(walkingSpeed, _player.GetHorizontalSpeed());
        }

        /// <summary>
        /// Set Attack flag on animator.
        /// </summary>
        public void HandleAttack()
        {
            animator.SetBool(isAttacking, true);
        }

        /// <summary>
        /// Set attack flag to false.
        /// </summary>
        public void HandleAttackRelease()
        {
            animator.SetBool(isAttacking, false);
        }

        /// <summary>
        /// Handles jump flags on animator.
        /// </summary>
        public void HandleJump()
        {
            animator.SetBool(isJumping, true);
            animator.SetBool(isFalling, true);
        }

        /// <summary>
        /// Resets animator flags.
        /// </summary>
        public void HandleDeath()
        {
            animator.SetBool(isJumping, false);
            animator.SetBool(isFalling, false);
            animator.SetBool(isJumping, false);
            animator.SetBool(isHanging, false);
        } 

        /// <summary>
        /// Handles player falling animator flags..
        /// </summary>
        public void HandleFall()
        {
            animator.SetBool(isJumping, false);
            animator.SetBool(isDoubleJumping, false);
            animator.SetBool(isFalling, true);
        }

        /// <summary>
        /// Handles double jump animator flags.
        /// </summary>
        public void HandleDoubleJump()
        {
            animator.SetBool(isDoubleJumping, true);
        }

        /// <summary>
        /// Handles in floor animator flags.
        /// </summary>
        public void HandleInFloor()
        {
            animator.SetBool(isFalling, false);
            animator.SetBool(isJumping, false);
            HandleWalk();
        }

        /// <summary>
        /// Handles edge grabbing animator flags.
        /// </summary>
        public void HandleHang()
        {
            animator.SetBool(isHanging, true);
        }

        /// <summary>
        /// Handles let go of edge grab animator flags.
        /// </summary>
        public void HandleLetGoHang()
        {
            animator.SetBool(isHanging, false);
        }
    }
}
