using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Create JumpCoinConfig", fileName = "JumpCoinConfig", order = 0)]
    public class JumpCoinConfig : ScriptableObject
    {
        public List<GameObject> noteObjects = new List<GameObject>();
        public List<Material> materialsList = new List<Material>();
    }
}
