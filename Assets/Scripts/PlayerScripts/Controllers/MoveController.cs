using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class MoveController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent OnMove;
        [SerializeField] private UnityEvent OnBreak;

        [Header("Locomotion")]
        [SerializeField] private float acceleration = 100;
        [SerializeField] private float maxSpeed = 10;
        [SerializeField] private float maxAcceleration = 50;
        [SerializeField] private AnimationCurve accelerationFactorFromDot;
        [SerializeField] private AnimationCurve maxAccelerationFactorFromDot;
        [SerializeField] private float brakeMultiplier = .60f;

        private Rigidbody _rigidbody;
        private Player _player;

        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;

        private Vector3 _goalVelocity;
        private bool _shouldBrake;
        public Vector3 Direction
        {
            get { return _desiredDirection; }
        }

        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _player ??= GetComponent<Player>();
        }

        public void LookChange()
        {
            MoveThowardsCamera();
        }

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

        public void Move(Vector3 direction)
        {
            _shouldBrake = direction.magnitude < 0.0001f;
            _obtainedDirection = direction;
            MoveThowardsCamera();
        }

        private void MoveThowardsCamera()
        {
            Transform localTransform = transform;
            var mainCamera = Camera.main;
            if (mainCamera != null)
                localTransform = mainCamera.transform;
            _desiredDirection = localTransform.TransformDirection(_obtainedDirection);
            _desiredDirection.y = 0;
        }

        private Vector3 GetForceToApply(bool isInAir = false)
        {
            Vector3 unitVelocity = GetHorizontalVelocity().normalized;

            float velocityDot = isInAir ? Vector3.Dot(_desiredDirection.normalized, unitVelocity) : 1;
            
            float accelerationToUse = acceleration * accelerationFactorFromDot.Evaluate(velocityDot);

            Vector3 goalVelocity = _desiredDirection.normalized * maxSpeed;

            _goalVelocity = Vector3.MoveTowards(
                _goalVelocity, 
                goalVelocity, 
                accelerationToUse * Time.fixedTime
            );

            Vector3 forceToApply = _goalVelocity - GetHorizontalVelocity();

            float maxAccelerationToUse = maxAcceleration * maxAccelerationFactorFromDot.Evaluate(velocityDot);

            return Vector3.ClampMagnitude(forceToApply, maxAccelerationToUse);
        }

        private Vector3 GetHorizontalVelocity()
        {
            return new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        }
        
        public void MovePlayer()
        {
            Vector3 forceToApply = GetForceToApply();
            
            Vector3 brakeForceVector = -GetHorizontalVelocity() * brakeMultiplier;
            
            _rigidbody.AddForce(forceToApply, ForceMode.Force);

            if (_shouldBrake)
            {
                _rigidbody.AddForce(brakeForceVector, ForceMode.Impulse);
                if(GetHorizontalVelocity().magnitude <= 0.0001f)
                    _shouldBrake = false;
            }
        }

        public void MovePlayerInAir()
        {
            Vector3 forceToApply = GetForceToApply(true);

            _rigidbody.AddForce(forceToApply, ForceMode.Force);
        }

        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawLine(
                    transform.position, 
                    transform.position + 
                        _rigidbody.velocity
                    );
                
            }
        }
    }
}
