using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class MoveController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent OnMove;
        [SerializeField] private UnityEvent OnBreak;

        [SerializeField] private float acceleration = 15;
        [SerializeField] private float brakeMultiplier = .60f;
        [SerializeField] private float breakAngle = 90f;
        [SerializeField] private float speed = 12;
        [SerializeField] [Range(1.0f, 5.0f)] private float touchingGroundImpulse = 2.0f;
        [SerializeField] [Range(0.01f, 20f)] private float movingInAirAcceleration;

        [SerializeField] private Rigidbody _rigidbody;
        private Player _player;

        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;
        private bool _shouldBrake;
        private bool _justTouchedGround = false;

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
            if (_rigidbody.velocity.magnitude > 0.0001f)
            {
                OnMove.Invoke();
            }
            else
            {
                OnBreak.Invoke();
            }
        }

        public void TouchesGround()
        {
            _justTouchedGround = true;
        }

        public void Move(Vector3 direction)
        {
            if (direction.magnitude < 0.0001f)
            {
                _shouldBrake = true;
            }
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

        public void MoveInGround()
        {
            Vector3 currentHorizontalVelocity = _rigidbody.velocity;
            currentHorizontalVelocity.y = 0;
            float currentSpeed = currentHorizontalVelocity.magnitude;
            
            Vector3 desiredForceToApply = _desiredDirection.normalized * acceleration;

            Vector3 brakeForceVector = -currentHorizontalVelocity * brakeMultiplier;
            
            _rigidbody.AddForce(desiredForceToApply, ForceMode.Force);

            if (_shouldBrake)
            {
                _rigidbody.AddForce(brakeForceVector, ForceMode.Impulse);
                _shouldBrake = false;
            }
            if (_justTouchedGround)
            {
                _rigidbody.AddForce(_desiredDirection.normalized * touchingGroundImpulse, ForceMode.Impulse);
                _justTouchedGround = false;
            }
        }

        public void MoveInAir()
        {
            Vector3 lastHorizontalVelocity = _rigidbody.velocity;
            lastHorizontalVelocity.y = 0;

            _rigidbody.AddForce(_desiredDirection.normalized * movingInAirAcceleration, ForceMode.Force);

            Vector3 newHorizontalVelocity = _rigidbody.velocity;
            newHorizontalVelocity.y = 0;

            _rigidbody.velocity = newHorizontalVelocity.normalized * lastHorizontalVelocity.magnitude + new Vector3(0, _rigidbody.velocity.y, 0);
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(
                transform.position, 
                transform.position + 
                    _rigidbody.velocity
                );
        }

        public void CheckVelocity()
        {
            Vector3 flatSpeed = _player.GetHorizontalVelocity();
            
            Vector3 limitedSpeed = flatSpeed.magnitude < speed ? flatSpeed : flatSpeed.normalized * speed;

            Vector3 directedSpeed = _desiredDirection.normalized * limitedSpeed.magnitude;
            
            _rigidbody.velocity = new Vector3(directedSpeed.x, _rigidbody.velocity.y, directedSpeed.z);
        }
    }
}