using System;
using System.Collections.Generic;
using Audio;
using Manager;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerAudio : MonoBehaviour
    {
        private Player _player;
        private bool _isPaused = false;
        
        [SerializeField] private string walkSound = "walk";
        [SerializeField] private string runSound = "run";
        [SerializeField] private string jumpSound = "jump";
        [SerializeField] private string doubleJumpSound = "doubleJump";
        [SerializeField] private string pauseMenuName = "pause";
        
        [Header("Events")] 
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
        
        void Start()
        {
            _player ??= GetComponent<Player>();
            
            EventManager.Instance.SubscribeTo(menuActivatedEvent, OnPause);
            EventManager.Instance.SubscribeTo(menuDeactivatedEvent, OnUnpause);
        }

        void OnDisable()
        {
            EventManager.Instance.UnsubscribeTo(menuActivatedEvent, OnPause);
            EventManager.Instance.UnsubscribeTo(menuDeactivatedEvent, OnUnpause);
        }

        private void OnPause(Dictionary<string, object> message)
        {
            Debug.Log("ASASFHADFH");
            Debug.Log((string) message["name"]);
            if ((string)message["name"] == pauseMenuName)
            {
                Debug.Log("Hi?");
                _isPaused = true;
            }
        }
        
        private void OnUnpause(Dictionary<string, object> message)
        {
            if ((string)message["name"] == pauseMenuName)
            {
                _isPaused = false;
            }
        }

        public void OnWalk()
        {
            if (_isPaused) return;
            
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
            if (_isPaused) return;

            AudioManager.Instance.StopSound(walkSound);
            AudioManager.Instance.StopSound(runSound);
        }

        public void OnJump()
        {
            if (_isPaused) return;

            AudioManager.Instance.StopSound(walkSound);
            AudioManager.Instance.StopSound(runSound);
            
            AudioManager.Instance.PlaySound(jumpSound);
        }

        public void OnDoubleJump()
        {
            if (_isPaused) return;

            AudioManager.Instance.PlaySound(doubleJumpSound);
        }
    }
}
