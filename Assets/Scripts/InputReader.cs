using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private WalkingBehaviour walkingBehaviour;
        [SerializeField] private RotationBehaviour rotationBehaviour;
        [SerializeField] private JumpBehaviour jumpBehaviour;

        private void Awake()
        {
            if (walkingBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(walkingBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(walkingBehaviour)} component!");
            }

            if (rotationBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(rotationBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(rotationBehaviour)} component!");
            }
        }

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            if (walkingBehaviour != null)
                walkingBehaviour.Move(moveDirection);
        }

        public void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            float angle = lookInput.x;
            if(walkingBehaviour != null && rotationBehaviour != null)
            {
                rotationBehaviour.RotateInAngles(angle);
                walkingBehaviour.LookChange();
            }
        }

        public void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(jumpBehaviour && context.started)
            {
                bool couldJump = jumpBehaviour.TryJump();
                walkingBehaviour.isJumping = couldJump;
            }
        }
    }
}
