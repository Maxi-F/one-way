using UnityEngine;

public class WithDebugRemover : MonoBehaviour
{
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