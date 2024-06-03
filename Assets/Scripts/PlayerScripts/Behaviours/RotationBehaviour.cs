using System;
using UnityEngine;

namespace PlayerScripts
{
    public class RotationBehaviour : MonoBehaviour
    {
        [SerializeField] private float rotationVelocity = 5.0f;

        private WalkingBehaviour _walkingBehaviour;
        private Vector3 _lookingDirection;
        private Vector3 _desiredLookingDirection = Vector3.zero;

        private void Start()
        {
            _walkingBehaviour ??= GetComponent<WalkingBehaviour>();
            _lookingDirection = transform.position + transform.forward;
        }

        public void Update()
        {
            if (_desiredLookingDirection == new Vector3(0, transform.position.y, 0)) return;

            _lookingDirection = Vector3.Lerp(_lookingDirection, _desiredLookingDirection, rotationVelocity * Time.deltaTime);
            _lookingDirection.y = transform.position.y;

            transform.LookAt(_lookingDirection);
        }

        public void LookInDirection()
        {
            if(_walkingBehaviour.direction == Vector3.zero) return;
            _desiredLookingDirection = transform.position + _walkingBehaviour.direction;
        }
    }
}
