using Audio;
using Manager;
using UnityEngine;

namespace UI.Button 
{
    public class BackToPlayButton : MonoBehaviour
    {
        [Header("Sounds")] [SerializeField] private string clickSound = "click";
        
        /// <summary>
        /// Handles click event.
        /// </summary>
        public void Click()
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            
            AudioManager.Instance?.PlaySound(clickSound);
            levelManager.TogglePause();
        }
    }
}
