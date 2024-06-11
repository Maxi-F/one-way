using PlayerScripts;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private CheatManager cheatManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private CameraBehaviour cameraBehaviour;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            if (cameraBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(cameraBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(cameraBehaviour)} component!");
            }
            
            if (player == null)
            {
                Debug.LogError($"{name}: {nameof(player)} is null!" +
                               $"\nThis class is dependant on a {nameof(player)} component!");
            }

            if (playerController == null)
            {
                Debug.LogError($"{name}: {nameof(playerController)} is null!" +
                               $"\nThis class is dependant on a {nameof(playerController)} component!");
            }

            if (cheatManager == null)
            {
                Debug.LogError($"{name}: {nameof(cheatManager)} is null!" +
                               $"\nThis class is dependant on a {nameof(cheatManager)} component!");
            }

            if(levelManager == null)
            {
                Debug.LogError($"{name}: {nameof(levelManager)} is null!" +
                               $"\nThis class is dependant on a {nameof(levelManager)} component!");
            }
        }

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            if (playerController != null)
                playerController.Move(moveDirection);
        }

        public void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            if(playerController != null)
            {
                cameraBehaviour.RotateCamera(lookInput);
                playerController.LookChange(lookInput);
            }
        }

        public void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(player && context.started)
            {
                player.Jump();
            }
        }

        public void HandlePowerJumpInput(InputAction.CallbackContext context)
        {
            if (player && context.started)
            {
                player.UseAccumulativeForceOnJump = true;
            } else if (player && context.canceled)
            {
                player.UseAccumulativeForceOnJump = false;
            }
        }

        public void HandleFly(InputAction.CallbackContext context)
        {
            if (cheatManager && context.started)
            {
                cheatManager.ToggleFly();
            }
        }

        public void HandlePassLevel(InputAction.CallbackContext context)
        {
            if (cheatManager && context.started)
            {
                cheatManager.PassLevel();
            }
        }

        public void HandlePause(InputAction.CallbackContext context)
        {
            if(levelManager && context.started)
            {
                levelManager.TogglePause();
            }
        }
    }
}
