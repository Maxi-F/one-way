using Manager;
using UnityEngine;

namespace Button 
{
    public class BackToPlayButton : MonoBehaviour
    {
        public void Click()
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            
            levelManager.TogglePause();
        }
    }
}
