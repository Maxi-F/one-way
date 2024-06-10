using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float jumpingMiliseconds = 100f;
        [SerializeField] private float coyoteTime = 0.5f;

        [SerializeField] private UnityEvent OnFalling;

        private float _timeJumped = 0f;
        private Player _player;
        private float _timePassedWithoutTouchingGround = 0f;

        private void Start()
        {
            _player ??= GetComponent<Player>();
        }

        private void OnUpdate()
        {
            if (!IsOnFloor())
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                if (_timePassedWithoutTouchingGround > coyoteTime)
                {
                    // Transition to Jump state
                    _player.Jump();

                    OnFalling.Invoke();
                }
            }
        }

        internal void ResetCoyoteTime()
        {
            _timePassedWithoutTouchingGround = 0f;
        }

        private bool JumpingBreakTime()
        {
            return _timeJumped + jumpingMiliseconds < (Time.time * 1000f);
        }

        private bool IsRaycastOnFloor()
        {
            return _player.CanJump();
        }

        public bool IsOnFloor()
        {
            return IsRaycastOnFloor() && JumpingBreakTime();
        }

        public bool CoyoteTimePassed()
        {
            return _timePassedWithoutTouchingGround < float.Epsilon || _timePassedWithoutTouchingGround > coyoteTime;
        }
    }

}
