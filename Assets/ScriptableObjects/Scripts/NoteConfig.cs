using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Create JumpCoinConfig", fileName = "JumpCoinConfig", order = 0)]
    public class NoteConfig : ScriptableObject
    {
        public List<GameObject> noteObjects = new List<GameObject>();
        public List<Material> materialsList = new List<Material>();
        public Vector3 noteScale = new Vector3(2, 2, 2);
        
        [Header("Sounds")] public List<string> sounds = new List<string>();
        
        [Header("Hover settings")]
        public float hoverVelocity = 0.5f;
        public float hoverDistance = 0.2f;
    }
}
