using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [Header("Behaviours")]
        [Header("PlayerData")]
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] WalkingBehaviour walkBehaviour;
        [SerializeField] private int lives = 3;
        [SerializeField] private int maxLives = 3;
        
        [Header("Accumulated force settings")] 
        [SerializeField] private float maxAccumulatedForce;
        
        public bool IsFlying { get; set; }

        public int Lives
        {
            get { return lives; }
        }
        
        private Rigidbody _rigidbody;
        private bool _shouldStop = false;
        private MovementFSM _movementFSM;
        
        private RotationBehaviour _rotationBehaviour;
        public float Sensibility { get; set; }
        
        public void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _rotationBehaviour ??= GetComponent<RotationBehaviour>();

            IBehaviour[] behaviours = GetComponents<IBehaviour>();
            _movementFSM = new MovementFSM(behaviours, walkBehaviour);

            lives = maxLives;
        }

        public Vector3 GetHorizontalVelocity()
        {
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = 0;

            return velocity;
        }

        public float GetHorizontalVelocityMagnitude()
        {
            return GetHorizontalVelocity().magnitude;
        }

        public float GetBoxSize()
        {
            return capsuleCollider.radius;
        }

        public void TouchesGround()
        {
            _movementFSM.changeStateTo(MovementBehaviour.Move);
        }

        public void Jump()
        {
            _movementFSM.changeStateTo(MovementBehaviour.Jump);
        }
        
        public void Update()
        {
            _movementFSM.OnUpdate();
            _rotationBehaviour.LookInDirection(); 
        }

        public void FixedUpdate()
        {
            if (_shouldStop)
            {
                _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Impulse);
                _shouldStop = false;
            }
            else {
                _movementFSM.OnFixedUpdate();
            }
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        public bool IsEdgeGrabbing()
        {
            return _movementFSM.IsCurrentBehaviour(MovementBehaviour.EdgeGrab);
        }

        public void SetGravity(bool active)
        {
            _rigidbody.useGravity = active;
        }

        public void EdgeGrab()
        {
            if (!IsEdgeGrabbing())
            {
                _movementFSM.changeStateTo(MovementBehaviour.EdgeGrab);
            }
        }

        public void Fly()
        {
            _movementFSM.changeStateTo(MovementBehaviour.Fly);
        }

        public void LoseLive()
        {
            lives--;
        }
    }
}
