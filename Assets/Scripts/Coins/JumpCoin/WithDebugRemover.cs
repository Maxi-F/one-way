using UnityEngine;

namespace Coins.JumpCoin
{
    public class WithDebugRemover : MonoBehaviour
    {
        /// <summary>
        /// Removes the debug capsule that is used in the scene editor.
        /// </summary>
        protected void RemoveDebug()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Debug"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}