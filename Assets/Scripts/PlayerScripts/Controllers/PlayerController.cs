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
        [SerializeField] Transform feetPivot;

        [Header("Events")]
        [SerializeField] private UnityEvent OnLand;
        [SerializeField] private UnityEvent OnFalling;

        private Player _player;
        private float _timePassedWithoutTouchingGround = 0f;
        private MoveController _moveController;
        private JumpController _jumpController;


        private void Start()
        {
            _player ??= GetComponent<Player>();
            _moveController ??= GetComponent<MoveController>();
            _jumpController ??= GetComponent<JumpController>();
        }

        public void Update()
        {
            if (!IsOnFloor())
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                Debug.Log(_timePassedWithoutTouchingGround);
                if (_timePassedWithoutTouchingGround > coyoteTime && !_jumpController.IsJumping)
                {
                    _player.Jump();
                    OnFalling.Invoke();
                }
            } else if (IsOnFloor() && _jumpController.IsJumping)
            {
                _player.TouchesGround();
                OnLand.Invoke();
            } else if (_player.IsOnEdge())
            {
                // TODO transition to edge grab state
                /*_player.SetBehaviour(_edgeGrabBehaviour);
                _edgeGrabBehaviour.SetEdgePosition(transform, player.GetForwardEdgeHit(), player.GetDownEdgeHit());
                IsJumping = false;
                */
            }
        }

        internal void ResetCoyoteTime()
        {
            if(IsOnFloor())
                _timePassedWithoutTouchingGround = 0f;
        }

        private bool JumpingBreakTime()
        {
            return _jumpController.TimeJumped + jumpingMiliseconds < (Time.time * 1000f);
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

        internal void Jump()
        {
            if(CanJump())
            {
                _player.Jump();
            }
        }

        public bool IsOnCoyoteTimeFloor()
        {
            return !CoyoteTimePassed() && JumpingBreakTime();
        }

        public bool CanJump()
        {
            return (feetPivot && IsRaycastOnFloor()) || IsOnCoyoteTimeFloor();
        }

        public Transform GetFeetPivot()
        {
            return feetPivot;
        }

        public void SetPowerJump(bool value)
        {
            _jumpController.UseAccumulativeForceOnJump = value;
        }

        public void Move(Vector3 direction)
        {
            _moveController.Move(direction);
        }

        public void LookChange(Vector2 eulers)
        {
            _moveController.LookChange();
        }
    }
}
