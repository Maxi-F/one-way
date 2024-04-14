using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class InputReader : MonoBehaviour
    {
        public WalkingBehaviour walkingBehaviour;

        private void Awake()
        {
            if (walkingBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(walkingBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(walkingBehaviour)} component!");
            }
        }

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            if (walkingBehaviour != null)
                walkingBehaviour.Move(moveDirection);
        }
    }
}
