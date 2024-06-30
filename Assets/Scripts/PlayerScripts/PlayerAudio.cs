using Audio;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerAudio : MonoBehaviour
    {
        private Player _player;
        
        [SerializeField] private string walkSound = "walk";
        [SerializeField] private string runSound = "run";

        void Start()
        {
            _player ??= GetComponent<Player>();
        }
        
        public void OnWalk()
        {
            Vector3 horizontalVelocity = _player.GetHorizontalVelocity();
            if (horizontalVelocity.magnitude < 0.0001f) return;

            if (horizontalVelocity.magnitude < _player.VelocityToRun)
            {
                AudioManager.Instance.StopSound(runSound);
                if (!AudioManager.Instance.IsPlayingSound(walkSound))
                {
                    AudioManager.Instance.PlaySound(walkSound);
                }
            }
            else
            {
                AudioManager.Instance.StopSound(walkSound);
                if (!AudioManager.Instance.IsPlayingSound(runSound))
                {
                    AudioManager.Instance.PlaySound(runSound);
                }
            }
            
            
        }

        public void OnStopWalk()
        {
            AudioManager.Instance.StopSound(walkSound);
            AudioManager.Instance.StopSound(runSound);
        }

        public void OnJump()
        {
            AudioManager.Instance.StopSound(walkSound);
            AudioManager.Instance.StopSound(runSound);
        }
    }
}
