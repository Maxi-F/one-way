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

        private Rigidbody _rigidbody;
        private Player _player;
        private PlayerController _playerController;

        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;
        private bool _shouldBrake;
        private bool _justTouchedGround = false;

        public Vector3 direction
        {
            get { return _desiredDirection; }
        }

        private void Reset()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _playerController ??= GetComponent<PlayerController>();
        }

        public void LookChange()
        {
            MoveThowardsCamera();
        }

        public void OnUpdate()
        {
            _playerController.ResetCoyoteTime();
            
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
            if ((direction.magnitude < 0.0001f || Vector3.Angle(direction, _player.GetHorizontalVelocity()) > breakAngle) && _playerController.IsOnFloor())
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

        public void OnFixedUpdate()
        {
            Vector3 currentHorizontalVelocity = _rigidbody.velocity;
            currentHorizontalVelocity.y = 0;
            float currentSpeed = currentHorizontalVelocity.magnitude;

            float angleBetweenVelocityAndDirection = Vector3.Angle(currentHorizontalVelocity, _desiredDirection);

            Vector3 desiredForceToApply = _desiredDirection.normalized * acceleration;

            Vector3 brakeForceVector = -currentHorizontalVelocity * brakeMultiplier;

            if (currentSpeed < speed)
            {
                _rigidbody.AddForce(desiredForceToApply, ForceMode.Force);
            }

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

        public void MoveInAir(float accelerationToUse)
        {
            Vector3 lastHorizontalVelocity = _rigidbody.velocity;
            lastHorizontalVelocity.y = 0;

            _rigidbody.AddForce(_desiredDirection.normalized * accelerationToUse, ForceMode.Force);

            Vector3 newHorizontalVelocity = _rigidbody.velocity;
            newHorizontalVelocity.y = 0;

            _rigidbody.velocity = newHorizontalVelocity.normalized * lastHorizontalVelocity.magnitude + new Vector3(0, _rigidbody.velocity.y, 0);
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + _rigidbody.velocity);
        }
    }
}
