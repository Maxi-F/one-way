using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SceneryManager _sceneryManager;
        private void Awake()
        {
            _sceneryManager.InitScenes();
        }
    }
}
