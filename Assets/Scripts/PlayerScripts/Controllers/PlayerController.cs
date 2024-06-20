using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float jumpingMiliseconds = 1000f;
        [SerializeField] private float coyoteTime = 0.5f;

        [Header("Events")]
        [SerializeField] private UnityEvent OnLand;
        [SerializeField] private UnityEvent OnFalling;
        [SerializeField] private UnityEvent OnEdgePositionSetted;
        [SerializeField] private UnityEvent OnEdgeJump;
        [SerializeField] private UnityEvent OnJump;
        
        private Player _player;
        private float _timePassedWithoutTouchingGround = 0f;
        private MoveController _moveController;
        private JumpController _jumpController;
        private EdgeGrabController _edgeGrabController;
        private FlyController _flyController;

        private bool _cheatsEnabled = false;
        
        private void Start()
        {
            _player ??= GetComponent<Player>();
            _moveController ??= GetComponent<MoveController>();
            _jumpController ??= GetComponent<JumpController>();
            _edgeGrabController ??= GetComponent<EdgeGrabController>();
            _flyController ??= GetComponent<FlyController>();
        }

        public void Update()
        {
            if (_cheatsEnabled) return;
            
            if (!IsOnFloor() && !_edgeGrabController.IsEdgeGrabbing)
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                if (_timePassedWithoutTouchingGround > coyoteTime && !_jumpController.IsJumping)
                {
                    _player.Jump();
                    OnFalling.Invoke();
                }
            } 
            
            if (IsOnFloor() && _jumpController.IsJumping)
            {
                _player.TouchesGround();
                OnLand.Invoke();
            } 
            
            if (_edgeGrabController.IsOnEdge())
            {
                _player.EdgeGrab();
                OnEdgePositionSetted.Invoke();
            }
        }

        internal void ResetCoyoteTime()
        {
            if(IsOnFloor())
                _timePassedWithoutTouchingGround = 0f;
        }

        public bool JumpingBreakTime()
        {
            return _jumpController.TimeJumped + jumpingMiliseconds < (Time.time * 1000f);
        }

        public bool IsOnFloor()
        {
            return _jumpController.CanJump() && JumpingBreakTime();
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
                else
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

        public void SetFly(bool value)
        {
            _flyController.GoDown = value;
        }

        public void Move(Vector3 direction)
        {
            _moveController.Move(direction);
        }

        public void LookChange(Vector2 eulers)
        {
            _moveController.LookChange();
        }

        public void OnCheatsToggle()
        {
            _cheatsEnabled = !_cheatsEnabled;
        }
    }
}
