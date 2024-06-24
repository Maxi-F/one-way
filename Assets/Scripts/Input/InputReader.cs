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

        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            OnMove?.Invoke(moveDirection);
        }

        public void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            
            Debug.Log(context.control.device);
            OnLook?.Invoke(lookInput, context.control.device != Mouse.current);
        }

        public void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnJump?.Invoke();
            }
        }

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

        public void HandleFly(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnFly?.Invoke();
            }
        }

        public void HandlePassLevel(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnPassLevel?.Invoke();
            }
        }

        public void HandlePause(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnPause?.Invoke();
            }
        }
    }
}
