using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] [Range(1.1f, 10f)] private float powerJumpImpulse = 2.0f;

        private PlayerController _playerController;
        private Rigidbody _rigidBody;
        
        private bool _shouldJump = false;
        private float _timeJumped = 0.0f;

        public float TimeJumped { get { return _timeJumped; } }

        public bool UseAccumulativeForceOnJump { get; set; }

        public bool IsJumping { get; set; }

        public void Start()
        {
            _playerController ??= GetComponent<PlayerController>();
            _rigidBody ??= GetComponent<Rigidbody>();
        }

        public void OnFixedUpdate()
        {
            if (_shouldJump)
            {
                if (UseAccumulativeForceOnJump)
                {
                    Vector3 upForce = (force + powerJumpImpulse) * Vector3.up;

                    Vector3 slowDownForce = -_rigidBody.velocity;
                    slowDownForce.y = 0;

                    _rigidBody.AddForce(upForce + slowDownForce, ForceMode.Impulse);
                }
                else
                {
                    _rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
                }
                _shouldJump = false;
                IsJumping = true;
            }
        }

        public void OnUpdate()
        {
        }

        public void SetShouldJumpValues()
        {
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
        }

        public void SetIsJumping(bool value = true)
        {
            IsJumping = value;
        }
    }
}
