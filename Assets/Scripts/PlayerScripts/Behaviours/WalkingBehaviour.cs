using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class WalkingBehaviour : MonoBehaviour, IBehaviour
    {
        [Header("Walking properties")]
        [SerializeField] private float speed = 12;
        [SerializeField] private float maxSpeedForDirChange = 4f;
        [SerializeField] private float acceleration = 15;
        [SerializeField] private float brakeMultiplier = .60f;
        [SerializeField] private float changeDirectionMultiplier = 1.25f;
        [SerializeField] private float maxAngleToChangeDirection = 45f;
        [SerializeField] private float coyoteTime = 0.5f;
        [SerializeField] private float breakAngle = 90f;
        
        [Header("Player Data")]
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Player player;
        
        [Header("Behaviours")]
        [SerializeField] private JumpBehaviour jumpBehaviour;

        [Header("Events")]
        [SerializeField] private UnityEvent OnMove;
        [SerializeField] private UnityEvent OnBreak;
        [SerializeField] private UnityEvent OnJump;
        [SerializeField] private UnityEvent OnFalling;
            
        private Vector3 _obtainedDirection;
        private Vector3 _desiredDirection;
        private bool _shouldBrake;
        private bool _justTouchedGround;
        private bool _isTouchingGround;
        private float _timePassedWithoutTouchingGround = 0f;

        public float CurrentSpeed
        {
            get
            {
                return _desiredDirection.magnitude * speed;
            }
        }

        public Vector3 direction
        {
            get { return _desiredDirection; }
        }

        private void Reset()
        {
            rigidBody ??= GetComponent<Rigidbody>();
            jumpBehaviour ??= GetComponent<JumpBehaviour>();
        }

        public void LookChange()
        {
            MoveThowardsCamera();
        }

        public void OnBehaviourUpdate()
        {
            if (!jumpBehaviour.IsOnFloor())
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                if(_timePassedWithoutTouchingGround > coyoteTime)
                {
                    player.SetBehaviour(jumpBehaviour); 
                    player.Jump();
                    OnFalling.Invoke();
                }
            }
            else
            {
                _timePassedWithoutTouchingGround = 0f;

                if(rigidBody.velocity.magnitude > 0.0001f)
                {
                    OnMove.Invoke();
                } else
                {
                    OnBreak.Invoke();
                }
            }
        }

        public void Move(Vector3 direction)
        {
            if ((direction.magnitude < 0.0001f || Vector3.Angle(direction, _desiredDirection) > breakAngle) && jumpBehaviour.IsOnFloor())
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

        public void TouchesGround()
        {
            _justTouchedGround = true;
        }

        public void Jump()
        {
            if (jumpBehaviour.IsOnFloor() || !CoyoteTimePassed())
            {
                player.AccumulateForce(GetCurrentHorizontalSpeed() / 2);
                player.SetBehaviour(jumpBehaviour);
                player.Jump();
                OnJump.Invoke();
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

            Vector3 desiredForceToApply = _desiredDirection.normalized * acceleration;

            Vector3 brakeForceVector = -currentHorizontalVelocity * brakeMultiplier;
            
            if (currentSpeed < speed)
            {
                rigidBody.AddForce(desiredForceToApply, ForceMode.Force);
            }

            if (_shouldBrake)
            {
                rigidBody.AddForce(brakeForceVector, ForceMode.Impulse);
                _shouldBrake = false;
            }

            if(_justTouchedGround)
            {
                rigidBody.AddForce(_desiredDirection.normalized * player.GetAccumulatedForceAndFlush(), ForceMode.Impulse);
                _justTouchedGround = false;
            }
        }

        public bool CoyoteTimePassed()
        {
            return _timePassedWithoutTouchingGround < float.Epsilon || _timePassedWithoutTouchingGround > coyoteTime;
        }

        public void ResetCoyoteTime()
        {
            _timePassedWithoutTouchingGround = 0f;
        }

        public void MoveInAir(float accelerationToUse)
        {
            Vector3 lastHorizontalVelocity = rigidBody.velocity;
            lastHorizontalVelocity.y = 0;
            
            Debug.Log(lastHorizontalVelocity.magnitude);
            
            rigidBody.AddForce(_desiredDirection.normalized * accelerationToUse, ForceMode.Force);

            Vector3 newHorizontalVelocity = rigidBody.velocity;
            newHorizontalVelocity.y = 0;
            
            rigidBody.velocity = newHorizontalVelocity.normalized * lastHorizontalVelocity.magnitude + new Vector3(0, rigidBody.velocity.y, 0);
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + rigidBody.velocity);
        }
    }
}
