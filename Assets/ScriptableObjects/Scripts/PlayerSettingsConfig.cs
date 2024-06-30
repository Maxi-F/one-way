using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Create PlayerSettings", fileName = "PlayerSettings", order = 0)]
    public class PlayerSettingsConfig : ScriptableObject
    {
        public float sensibility = 0.1f;
        public float sfxVolume = 1f;
        public float musicVolume = 1f;
    }
}
