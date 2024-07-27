using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts.Controllers
{
    public class MoveController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent OnMove;
        [SerializeField] private UnityEvent OnBreak;
        [SerializeField] private string onFlashToggle = "onFlashToggle";

        [Header("Locomotion")]
        [SerializeField] private float acceleration = 100;
        [SerializeField] private float maxSpeed = 10;
        [SerializeField] private float maxAcceleration = 50;
        [SerializeField] private AnimationCurve accelerationFactorFromDot;
        [SerializeField] private AnimationCurve maxAccelerationFactorFromDot;
        [SerializeField] private float brakeMultiplier = .60f;

        [Header("Cheat locomotion")]
        [SerializeField] private float cheatAcceleration = 1000;
        [SerializeField] private float cheatMaxSpeed = 100;
        [SerializeField] private float cheatMaxAcceleration = 500;

        private bool _isFlashToggled = false;
        
        private Rigidbody _rigidBody;
        private Player _player;

        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;

        private Vector3 _goalVelocity;
        private bool _shouldBrake;
        private float _maxSpeedToUse;
        private float _maxAccelerationToUse;
        private float _accelerationToUse;
        public Vector3 Direction => _desiredDirection;

        private void Start()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
            _player ??= GetComponent<Player>();

            _maxAccelerationToUse = maxAcceleration;
            _accelerationToUse = acceleration;
            _maxSpeedToUse = maxSpeed;
            
            EventManager.Instance.SubscribeTo(onFlashToggle, HandleFlashToggle);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnsubscribeTo(onFlashToggle, HandleFlashToggle);
        }

        private void HandleFlashToggle(Dictionary<string, object> message)
        {
            if (!_isFlashToggled)
            {
                _maxSpeedToUse = cheatMaxSpeed;
                _maxAccelerationToUse = cheatMaxAcceleration;
                _accelerationToUse = cheatAcceleration;
            }
            else
            {
                _maxAccelerationToUse = maxAcceleration;
                _accelerationToUse = acceleration;
                _maxSpeedToUse = maxSpeed;
            }

            _isFlashToggled = !_isFlashToggled;
        }

        /// <summary>
        /// Makes the player change look direction
        /// </summary>
        public void LookChange()
        {
            MoveThowardsCamera();
        }

        /// <summary>
        /// Executes movement events.
        /// </summary>
        public void Events()
        {
            if (GetHorizontalVelocity().magnitude > 0.0001f)
            {
                OnMove.Invoke();
            }
            else
            {
                OnBreak.Invoke();
            }
        }

        /// <summary>
        /// Changes the player's desired direction movement
        /// </summary>
        /// <param name="direction">new desired direction</param>
        public void Move(Vector3 direction)
        {
            _shouldBrake = direction.magnitude < 0.0001f;
            _obtainedDirection = direction;
            MoveThowardsCamera();
        }

        /// <summary>
        /// Makes the player move to the desired direction depending on the camera
        /// position
        /// </summary>
        private void MoveThowardsCamera()
        {
            Transform localTransform = transform;
            var mainCamera = Camera.main;
            if (mainCamera != null)
                localTransform = mainCamera.transform;
            _desiredDirection = localTransform.TransformDirection(_obtainedDirection);
            _desiredDirection.y = 0;
        }

        /// <summary>
        /// Returns the force to apply on the frame for the movement.
        /// It takes into account:
        ///     - Current velocity and desired velocity
        ///     - a factor of acceleration depending on the angle between the current velocity and desired direction
        /// </summary>
        /// <param name="isInAir">bool that checks if the player is in air or not.</param>
        /// <returns></returns>
        private Vector3 GetForceToApply(bool isInAir = false)
        {
            Vector3 unitVelocity = GetHorizontalVelocity().normalized;

            float velocityDot = isInAir ? Vector3.Dot(_desiredDirection.normalized, unitVelocity) : 1;
            
            float accelerationToUse = _accelerationToUse * accelerationFactorFromDot.Evaluate(velocityDot);

            Vector3 goalVelocity = _desiredDirection.normalized * _maxSpeedToUse;
            
            _goalVelocity = Vector3.MoveTowards(
                _goalVelocity, 
                goalVelocity, 
                accelerationToUse * Time.fixedTime
            );

            Vector3 forceToApply = _goalVelocity - GetHorizontalVelocity();

            Debug.Log(_desiredDirection.normalized);
            
            float maxAccelerationToUse = _maxAccelerationToUse * maxAccelerationFactorFromDot.Evaluate(velocityDot);

            return Vector3.ClampMagnitude(forceToApply, maxAccelerationToUse);
        }

        /// <summary>
        /// Returns the current horizontal velocity of the player.
        /// </summary>
        private Vector3 GetHorizontalVelocity()
        {
            return new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        }
        
        /// <summary>
        /// on fixed update of the player movement.
        /// Uses the force to apply and a brake vector if the player is not moving anymore.
        /// </summary>
        public void MovePlayer()
        {
            Vector3 forceToApply = GetForceToApply();
            
            Vector3 brakeForceVector = -GetHorizontalVelocity() * brakeMultiplier;
            
            _rigidBody.AddForce(forceToApply, ForceMode.Force);

            if (_shouldBrake)
            {
                _rigidBody.AddForce(brakeForceVector, ForceMode.Impulse);
                if(GetHorizontalVelocity().magnitude <= 0.0001f)
                    _shouldBrake = false;
            }
        }

        /// <summary>
        /// On fixed update movement of the player on air.
        /// </summary>
        public void MovePlayerInAir()
        {
            Vector3 forceToApply = GetForceToApply(true);

            _rigidBody.AddForce(forceToApply, ForceMode.Force);
        }

        public void OnDrawGizmos()
        {

            if (Application.isPlaying && _rigidBody != null)
            {
                Gizmos.DrawLine(
                    transform.position, 
                    transform.position + 
                        _rigidBody.velocity
                    );
                
            }
        }
        
    }
}
