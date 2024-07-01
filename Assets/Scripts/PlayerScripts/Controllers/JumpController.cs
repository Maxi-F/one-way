using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlayerScripts.Controllers
{
    public class JumpController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float force;
        [SerializeField] private float jumpingMilliseconds = 1000f;
        [SerializeField] [Min(1)] private int maxDoubleJumpsFromGround;
        [SerializeField] private float maxTimeStuckFalling = 0.5f;
        [SerializeField] private float stuckYVelocity = 0.001f;
        
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
        private float _timeStuckFalling = 0.0f;
        public bool IsJumping { get; set; }

        public void Start()
        {
            _jumpsLeft = maxDoubleJumpsFromGround;
            
            EventManager.Instance?.TriggerEvent(resetJumpValuesEvent, new Dictionary<string, object>() { {"value", _jumpsLeft} } );

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
            else if(_timeStuckFalling > maxTimeStuckFalling)
            {
                _rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Returns the velocity of the rigid body only in Y direction.
        /// </summary>
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

            float yVelocity = GetYVelocityVector().y;
            if (yVelocity < stuckYVelocity && yVelocity > -stuckYVelocity)
            {
                _timeStuckFalling += Time.deltaTime;
            }
            else
            {
                _timeStuckFalling = 0;
            }
        }

        /// <summary>
        /// Sets the init values of the jumping controller on player jump.
        /// </summary>
        public void SetShouldJumpValues()
        {
            _jumpsLeft = maxDoubleJumpsFromGround;
            
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
        }

        /// <summary>
        /// Sets the controller Is Jumping flag
        /// </summary>
        /// <param name="value">boolean to set the jumping flag to</param>
        public void SetIsJumping(bool value = true)
        {
            IsJumping = value;
        }

        /// <summary>
        /// Returns true if player has jumps left.
        /// </summary>
        public bool CanJump()
        {
            return _jumpsLeft > 0;
        }

        /// <summary>
        /// Makes the double jump on the air.
        /// </summary>
        public void JumpFromAir()
        {
            _jumpsLeft--;
            
            EventManager.Instance?.TriggerEvent(modifyJumpValuesEvent, new Dictionary<string, object>() { {"value", -1} } );
            
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
            OnDoubleJump?.Invoke();
        }

        /// <summary>
        /// Returns if the player can make another jump after a cooldown.
        /// </summary>
        public bool JumpingBreakTime()
        {
            return _timeJumped < 0.0001f || _timeJumped + jumpingMilliseconds < (Time.time * 1000f);
        }

        /// <summary>
        /// Checks if the player is on the floor, taking into account a cooldown.
        /// </summary>
        public bool IsOnFloor()
        {
            return _groundController.IsOnGround() && JumpingBreakTime();
        }
        
        /// <summary>
        /// Resets the double jumps that the player can do.
        /// </summary>
        public void ResetJumps()
        {
            _jumpsLeft = maxDoubleJumpsFromGround;
            EventManager.Instance?.TriggerEvent(resetJumpValuesEvent, new Dictionary<string, object>() { {"value", maxDoubleJumpsFromGround} } );
        }

        /// <summary>
        /// Adds a new jump on air.
        /// </summary>
        public void AddJump()
        {
            if(IsJumping)
                _jumpsLeft++;
        }
    }
}
