using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Create TomatoConfig", fileName = "TomatoConfig", order = 0)]
    public class TomatoConfig : ScriptableObject
    {
        public GameObject tomatoObject;
        public string tomatoSound;
    }
}
