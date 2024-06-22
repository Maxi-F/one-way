using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create PlayerSettings", fileName = "PlayerSettings", order = 0)]
    public class SensibilitySettings : ScriptableObject
    {
        public float sensibility = 0.1f;
    }
}
