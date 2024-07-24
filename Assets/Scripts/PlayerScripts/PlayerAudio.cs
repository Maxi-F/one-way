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
        
        [Header("Sounds")]
        [SerializeField] private string walkSound = "walk";
        [SerializeField] private string runSound = "run";
        [SerializeField] private string jumpSound = "jump";
        [SerializeField] private string doubleJumpSound = "doubleJump";
        [SerializeField] private string pauseMenuName = "pause";
        [SerializeField] private string attackSound = "attack";
        
        [Header("Events")] 
        [SerializeField] private string menuActivatedEvent = "menuActivated";
        [SerializeField] private string menuDeactivatedEvent = "menuDeactivated";
        
        void Start()
        {
            _player ??= GetComponent<Player>();
            
            EventManager.Instance?.SubscribeTo(menuActivatedEvent, OnPause);
            EventManager.Instance?.SubscribeTo(menuDeactivatedEvent, OnUnpause);
        }

        void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(menuActivatedEvent, OnPause);
            EventManager.Instance?.UnsubscribeTo(menuDeactivatedEvent, OnUnpause);
        }

        /// <summary>
        /// Event that listens to pause event to pause playing sounds.
        /// </summary>
        private void OnPause(Dictionary<string, object> message)
        {
            if ((string)message["name"] == pauseMenuName)
            {
                _isPaused = true;
            }
        }
        
        /// <summary>
        /// Event that listens to pause event to resume playing sounds.
        /// </summary>
        private void OnUnpause(Dictionary<string, object> message)
        {
            if (message.ContainsKey("isDeactivating") && (bool)message["isDeactivating"]) return;
            if ((string)message["name"] == pauseMenuName)
            {
                _isPaused = false;
            }
        }

        public void HandleAttack()
        {
            if (_isPaused) return;
            
            AudioManager.Instance?.StopSound(walkSound);
            AudioManager.Instance?.StopSound(runSound);
            
            AudioManager.Instance?.PlaySound(attackSound);
        }

        /// <summary>
        /// Handles Walk sounds or running sounds.
        /// </summary>
        public void OnWalk()
        {
            if (_isPaused || _player.IsAttacking()) return;
            
            Vector3 horizontalVelocity = _player.GetHorizontalVelocity();
            if (horizontalVelocity.magnitude < 0.0001f) return;

            if (horizontalVelocity.magnitude < _player.VelocityToRun)
            {
                AudioManager.Instance?.StopSound(runSound);
                if (AudioManager.Instance != null && !AudioManager.Instance.IsPlayingSound(walkSound))
                {
                    AudioManager.Instance?.PlaySound(walkSound);
                }
            }
            else
            {
                AudioManager.Instance?.StopSound(walkSound);
                if (AudioManager.Instance != null && !AudioManager.Instance.IsPlayingSound(runSound))
                {
                    AudioManager.Instance?.PlaySound(runSound);
                }
            }
        }

        /// <summary>
        /// Handles stop walking sounds.
        /// </summary>
        public void OnStopWalk()
        {
            if (_isPaused) return;

            AudioManager.Instance?.StopSound(walkSound);
            AudioManager.Instance?.StopSound(runSound);
        }

        /// <summary>
        /// Handles jump sounds.
        /// </summary>
        public void OnJump()
        {
            if (_isPaused) return;

            AudioManager.Instance?.StopSound(walkSound);
            AudioManager.Instance?.StopSound(runSound);
            
            AudioManager.Instance?.PlaySound(jumpSound);
        }

        /// <summary>
        /// Handles double jump sounds
        /// </summary>
        public void OnDoubleJump()
        {
            if (_isPaused) return;

            AudioManager.Instance?.PlaySound(doubleJumpSound);
        }
    }
}
