using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class MenuInputReader : MonoBehaviour
    {
        public void HandleUIMoveInput(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log(input);
        }
    }
}
