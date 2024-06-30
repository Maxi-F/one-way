using Audio;
using Manager;
using UnityEngine;

namespace Button 
{
    public class BackToPlayButton : MonoBehaviour
    {
        [Header("Sounds")] [SerializeField] private string clickSound = "click";
        
        public void Click()
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            
            AudioManager.Instance.PlaySound(clickSound);
            levelManager.TogglePause();
        }
    }
}
