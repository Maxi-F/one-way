using UnityEngine;

namespace Button 
{
    public class BackToPlayButton : MonoBehaviour
    {
        [SerializeField] private GameObject pauseCanvas;
        
        public void Click()
        {
            if (pauseCanvas != null)
            {
                pauseCanvas.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}
