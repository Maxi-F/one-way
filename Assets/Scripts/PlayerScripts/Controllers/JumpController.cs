using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace PlayerScripts
{
    public class JumpController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float force;
        [SerializeField] private float jumpingMiliseconds = 1000f;
        [SerializeField] [Min(1)] private int maxDoubleJumpsFromGround;
        
        [Header("Events")]
        [SerializeField] private UnityEvent OnLand;
        [SerializeField] private UnityEvent OnDoubleJump;
        [SerializeField] private string resetJumpValuesEvent = "initNotes";
        [SerializeField] private string modifyJumpValuesEvent = "noteModified";
        
        private Rigidbody _rigidBody;
        private Player _player;
        private GroundController _groundController;
        
        private bool _shouldJump = false;
        private int _jumpsLeft;
        private float _timeJumped = 0.0f;

        public float TimeJumped { get { return _timeJumped; } }
        
        public bool IsJumping { get; set; }

        public void Start()
        {
            _jumpsLeft = maxDoubleJumpsFromGround;
            
            EventManager.Instance.TriggerEvent(resetJumpValuesEvent, new Dictionary<string, object>() { {"value", _jumpsLeft} } );

            _player ??= GetComponent<Player>();
            _rigidBody ??= GetComponent<Rigidbody>();
            _groundController ??= GetComponent<GroundController>();
        }

        public void OnFixedUpdate()
        {
            if (_shouldJump)
            {
                Vector3 forceToApply = (Vector3.up * force) - GetYVelocityVector();
                
                _rigidBody.AddForce(forceToApply, ForceMode.Impulse);
                _shouldJump = false;
                IsJumping = true;
            }
        }

        private Vector3 GetYVelocityVector()
        {
            return new Vector3(0, _rigidBody.velocity.y, 0);
        }

        public void OnUpdate()
        {
            if (IsOnFloor() && IsJumping)
            {
                _player.TouchesGround();
                OnLand.Invoke();
            }
        }

        public void SetShouldJumpValues()
        {
            _jumpsLeft = _groundController.IsOnGround() ? maxDoubleJumpsFromGround : maxDoubleJumpsFromGround - 1;
            
            if(!_groundController.IsOnGround())
                EventManager.Instance.TriggerEvent(modifyJumpValuesEvent, new Dictionary<string, object>() { {"value", -1} } );
            
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
        }

        public void SetIsJumping(bool value = true)
        {
            IsJumping = value;
        }

        public bool CanJump()
        {
            return _jumpsLeft > 0;
        }

        public void JumpFromAir()
        {
            _jumpsLeft--;
            
            EventManager.Instance.TriggerEvent(modifyJumpValuesEvent, new Dictionary<string, object>() { {"value", -1} } );
            
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
            OnDoubleJump?.Invoke();
        }

        public bool JumpingBreakTime()
        {
            return TimeJumped < 0.0001f || TimeJumped + jumpingMiliseconds < (Time.time * 1000f);
        }

        public bool IsOnFloor()
        {
            return _groundController.IsOnGround() && JumpingBreakTime();
        }

        public void ResetJumps()
        {
            _jumpsLeft = maxDoubleJumpsFromGround;
            EventManager.Instance.TriggerEvent(resetJumpValuesEvent, new Dictionary<string, object>() { {"value", maxDoubleJumpsFromGround} } );
        }

        public void AddJump()
        {
            if(IsJumping)
                _jumpsLeft++;
        }
    }
}
