using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float coyoteTime = 0.5f;

        [Header("Events")]
        [SerializeField] private UnityEvent OnFalling;
        [SerializeField] private UnityEvent OnEdgeJump;
        [SerializeField] private UnityEvent OnJump;
        
        private Player _player;
        private float _timePassedWithoutTouchingGround = 0f;
        private MoveController _moveController;
        private JumpController _jumpController;
        private EdgeGrabController _edgeGrabController;
        private FlyController _flyController;

        private bool _cheatsEnabled = false;
        private bool _isFalling = false;
        
        private void Start()
        {
            _isFalling = false;
            
            _player ??= GetComponent<Player>();
            _moveController ??= GetComponent<MoveController>();
            _jumpController ??= GetComponent<JumpController>();
            _edgeGrabController ??= GetComponent<EdgeGrabController>();
            _flyController ??= GetComponent<FlyController>();
        }

        public void Update()
        {
            if (!IsOnFloor() && !_edgeGrabController.IsEdgeGrabbing)
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                if (_timePassedWithoutTouchingGround > coyoteTime)
                {
                    if (!_jumpController.IsJumping)
                    {
                        _player.Jump();
                    }
                    if (!_isFalling)
                    {
                        OnFalling.Invoke();
                    }
                    _isFalling = true;
                }
            }

            if (IsOnFloor())
            {
                _isFalling = false;
            }
        }

        internal void ResetCoyoteTime()
        {
            if(IsOnFloor())
                _timePassedWithoutTouchingGround = 0f;
        }

        public bool JumpingBreakTime()
        {
            return _jumpController.JumpingBreakTime();
        }

        public bool IsOnFloor()
        {
            return _jumpController.IsOnFloor();
        }

        public bool CoyoteTimePassed()
        {
            return _timePassedWithoutTouchingGround < float.Epsilon || _timePassedWithoutTouchingGround > coyoteTime;
        }

        public void Jump()
        {
            if (_cheatsEnabled)
            {
                _flyController.GoUp();
                return;
            }
            
            if(CanJump())
            {
                if (_edgeGrabController.IsEdgeGrabbing)
                {
                    OnEdgeJump?.Invoke();
                }
                else if(IsOnFloor())
                {
                    OnJump?.Invoke();
                }
                
                _player.Jump();
            }
        }

        public bool IsOnCoyoteTimeFloor()
        {
            return !CoyoteTimePassed() && JumpingBreakTime();
        }

        public bool CanJump()
        {
            return _jumpController.CanJump() || IsOnCoyoteTimeFloor() || _edgeGrabController.IsEdgeGrabbing;
        }

        public void HandleDoubleJump()
        {
            _isFalling = false;
        }

        public void SetFly(bool value)
        {
            _flyController.GoDown = value;
        }

        public void Move(Vector3 direction)
        {
            _moveController.Move(direction);
        }

        public void LookChange(Vector2 eulers, bool isController)
        {
            _moveController.LookChange();
        }

        public void OnCheatsToggle()
        {
            _cheatsEnabled = !_cheatsEnabled;
        }

        public void AddJump()
        {
            _jumpController.AddJump();
        }
    }
}
