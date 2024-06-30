using UnityEngine;

namespace PlayerScripts.Behaviours
{
    public class RotationBehaviour : MonoBehaviour
    {
        [SerializeField] private float rotationVelocity = 5.0f;

        private MoveController _moveController;
        private Vector3 _lookingDirection;
        private Vector3 _desiredLookingDirection = Vector3.zero;

        private void Start()
        {
            _moveController ??= GetComponent<MoveController>();
            _lookingDirection = transform.forward;
        }

        public void Update()
        {
            if (_desiredLookingDirection == new Vector3(0, transform.position.y, 0)) return;

            _lookingDirection = Vector3.Lerp(_lookingDirection, _desiredLookingDirection, rotationVelocity * Time.deltaTime);

            transform.LookAt(transform.position + _lookingDirection);
        }

        /// <summary>
        /// Sets the desired look direction to the direction the move controller wants to go.
        /// </summary>
        public void LookInDirection()
        {
            if(_moveController.Direction == Vector3.zero) return;
            _desiredLookingDirection = _moveController.Direction;
        }
    }
}
