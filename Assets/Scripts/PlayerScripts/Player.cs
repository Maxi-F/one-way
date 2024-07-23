using System;
using PlayerScripts.Behaviours;
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
        [SerializeField] private int velocityToRun = 10;
        
        [Header("Accumulated force settings")] 
        [SerializeField] private float maxAccumulatedForce;
        
        public bool IsFlying { get; set; }

        public int VelocityToRun
        {
            get { return velocityToRun; }
        }

        public int Lives
        {
            get { return lives; }
        }
        
        private Rigidbody _rigidBody;
        private bool _shouldStop = false;
        private MovementFsm _movementFSM;
        
        private RotationBehaviour _rotationBehaviour;
        public float Sensibility { get; set; }
        
        public void Start()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
            _rotationBehaviour ??= GetComponent<RotationBehaviour>();

            IBehaviour[] behaviours = GetComponents<IBehaviour>();
            _movementFSM = new MovementFsm(behaviours, walkBehaviour);

            lives = maxLives;
        }

        /// <summary>
        /// Returns the current player horizontal velocity.
        /// </summary>
        public Vector3 GetHorizontalVelocity()
        {
            Vector3 velocity = _rigidBody.velocity;
            velocity.y = 0;

            return velocity;
        }

        /// <summary>
        /// Returns the current player horizontal velocity magnitude.
        /// </summary>
        public float GetHorizontalVelocityMagnitude()
        {
            return GetHorizontalVelocity().magnitude;
        }

        /// <summary>
        /// Makes the player change its state to move.
        /// </summary>
        public void TouchesGround()
        {
            _movementFSM.ChangeStateTo(MovementBehaviour.Move);
        }

        /// <summary>
        /// Makes the player change its state to jump.
        /// </summary>
        public void Jump()
        {
            _movementFSM.ChangeStateTo(MovementBehaviour.Jump);
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
                _rigidBody.AddForce(-_rigidBody.velocity, ForceMode.Impulse);
                _shouldStop = false;
            }
            else {
                _movementFSM.OnFixedUpdate();
            }
        }

        /// <summary>
        /// Makes the player stop movement.
        /// </summary>
        public void Stop()
        {
            _shouldStop = true;
        }

        /// <summary>
        /// Checks if player is edge grabbing or not.
        /// </summary>
        public bool IsEdgeGrabbing()
        {
            return _movementFSM.IsCurrentBehaviour(MovementBehaviour.EdgeGrab);
        }

        
        /// <summary>
        /// Makes the player edge grab if it was not edge grabbing before.
        /// </summary>
        public void EdgeGrab()
        {
            if (!IsEdgeGrabbing())
            {
                _movementFSM.ChangeStateTo(MovementBehaviour.EdgeGrab);
            }
        }

        /// <summary>
        /// Changes the player state to fly.
        /// </summary>
        public void Fly()
        {
            _movementFSM.ChangeStateTo(MovementBehaviour.Fly);
        }

        /// <summary>
        /// makes a player lose a life
        /// </summary>
        public void LoseLive()
        {
            lives--;
        }
    }
}
