using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Player player;
        private void Awake()
        {
            if (player == null)
            {
                Debug.LogError($"{name}: {nameof(player)} is null!" +
                               $"\nThis class is dependant on a {nameof(player)} component!");
            }
        }

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            if (player != null)
                player.Move(moveDirection);
        }

        public void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            if(player != null)
            {
                player.LookChange(lookInput);
            }
        }

        public void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(player && context.started)
            {
                player.Jump();
            }
        }
    }
}
