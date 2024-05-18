using UnityEngine;

namespace PlayerScripts
{
    public class WalkingBehaviour : MonoBehaviour, IBehaviour
    {
        [SerializeField] private float speed = 12;
        [SerializeField] private float acceleration = 15;
        [SerializeField] private float brakeMultiplier = .60f;
        [SerializeField] private float changeDirectionMultiplier = 1.25f;
        [SerializeField] private float maxAngleToChangeDirection = 45f;

        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Player player;
        [SerializeField] private JumpBehaviour jumpBehaviour;

        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;
        private bool _shouldBrake;
        private bool _isTouchingGround;

        public float CurrentSpeed
        {
            get
            {
                return _desiredDirection.magnitude * speed;
            }
        }

        private void Reset()
        {
            rigidBody ??= GetComponent<Rigidbody>();
            jumpBehaviour ??= GetComponent<JumpBehaviour>();
        }

        public void LookChange()
        {
            _desiredDirection = transform.TransformDirection(_obtainedDirection);
            _desiredDirection.y = 0;
        }

        public void OnBehaviourUpdate()
        {
            if (!jumpBehaviour.IsOnFloor())
            {
                player.SetBehaviour(jumpBehaviour);
                player.Jump();
            }
        }

        public void Move(Vector3 direction)
        {
            if (direction.magnitude < 0.0001f && jumpBehaviour.IsOnFloor())
            {
                _shouldBrake = true;
            }
            _obtainedDirection = direction;
            Transform localTransform = transform;
            var mainCamera = Camera.main;
            if (mainCamera != null)
                localTransform = mainCamera.transform;
            _desiredDirection = localTransform.TransformDirection(_obtainedDirection);
            _desiredDirection.y = 0;
        }

        public void TouchesGround()
        {
            _isTouchingGround = true;
        }

        public void Jump()
        {
            if (jumpBehaviour.IsOnFloor())
            {
                player.AccumulateForce(GetCurrentHorizontalSpeed() / 2);
                player.SetBehaviour(jumpBehaviour);
                player.Jump();
            }
        }

        public string GetName()
        {
            return "Walking Behaviour";
        }

        private float GetCurrentHorizontalSpeed()
        {
            Vector3 currentHorizontalVelocity = rigidBody.velocity;
            currentHorizontalVelocity.y = 0;
            return currentHorizontalVelocity.magnitude;
        }
        
        public void OnBehaviourFixedUpdate()
        {
            Vector3 currentHorizontalVelocity = rigidBody.velocity;
            currentHorizontalVelocity.y = 0;
            float currentSpeed = currentHorizontalVelocity.magnitude;

            float angleBetweenVelocityAndDirection = Vector3.Angle(currentHorizontalVelocity, _desiredDirection);

            if (currentSpeed < speed)
                rigidBody.AddForce(
                    _desiredDirection.normalized * 
                    (acceleration * 
                     (angleBetweenVelocityAndDirection > maxAngleToChangeDirection ? changeDirectionMultiplier : 1.0f)),
                    ForceMode.Force
                );
            if (_shouldBrake)
            {
                rigidBody.AddForce(-currentHorizontalVelocity * brakeMultiplier, ForceMode.Impulse);
                _shouldBrake = false;
            }
            if(_isTouchingGround)
            {
                rigidBody.AddForce(_desiredDirection.normalized * player.GetAccumulatedForceAndFlush(), ForceMode.Impulse);
                _isTouchingGround = false;
            }
        }
    }
}
