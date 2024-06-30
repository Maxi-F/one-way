using PlayerScripts;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Vector3> OnMove;
        [SerializeField] private UnityEvent<Vector2, bool> OnLook;
        [SerializeField] private UnityEvent OnJump;
        [SerializeField] private UnityEvent<bool> OnGoDown;
        [SerializeField] private UnityEvent OnFly;
        [SerializeField] private UnityEvent OnPassLevel;
        [SerializeField] private UnityEvent OnPause;

        /// <summary>
        /// Handles move input events from input actions.
        /// </summary>
        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            OnMove?.Invoke(moveDirection);
        }

        /// <summary>
        /// Handles look input events from input actions.
        /// </summary>
        public void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            
            OnLook?.Invoke(lookInput, context.control.device != Mouse.current);
        }

        /// <summary>
        /// Handles jump input events from input actions.
        /// </summary>
        public void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnJump?.Invoke();
            }
        }

        /// <summary>
        /// Handles Fly down events from input actions.
        /// </summary>
        public void HandleGoDownInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnGoDown?.Invoke(true);
            } else if (context.canceled)
            {
                OnGoDown?.Invoke(false);
            }
        }

        /// <summary>
        /// Handles Fly input events from input actions.
        /// </summary>
        public void HandleFly(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnFly?.Invoke();
            }
        }

        /// <summary>
        /// Handles pass level input events from input actions.
        /// </summary>
        public void HandlePassLevel(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnPassLevel?.Invoke();
            }
        }

        /// <summary>
        /// Handles pause input events from input actions.
        /// </summary>
        public void HandlePause(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnPause?.Invoke();
            }
        }
    }
}
