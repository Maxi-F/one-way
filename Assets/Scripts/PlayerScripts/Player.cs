using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        private IBehaviour _behaviour;
        [Header("Behaviours")]
        [FormerlySerializedAs("_rotationBehaviour")] [SerializeField] private RotationBehaviour rotationBehaviour;
        
        [Header("PlayerData")]
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private LayerMask floor;
        [SerializeField] Transform feetPivot;
        
        [Header("Jump Settings")]
        [SerializeField] private float groundedDistance = 0.1f;

        [Header("Accumulated force settings")] 
        [SerializeField] private float maxAccumulatedForce;
        
        private Rigidbody _rigidbody;
        private bool _shouldStop = false;
        private float _accumulatedForce = 0f;
        public bool useAccumulativeForceOnJump { get; set; }

        public void Awake()
        {
            if (rotationBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(rotationBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(rotationBehaviour)} component!");
            }
        }
        public void Start()
        {
            _behaviour ??= GetComponent<WalkingBehaviour>();
            rotationBehaviour ??= GetComponent<RotationBehaviour>();
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public float GetBoxSize()
        {
            return capsuleCollider.radius;
        }

        public void SetBehaviour(IBehaviour newBehaviour)
        {
            _behaviour = newBehaviour;
        }

        public bool IsRaycastOnFloor()
        {
            return Physics.Raycast(feetPivot.position, Vector3.down, out var hit, groundedDistance, floor);
        }
        
        public void Move(Vector3 direction)
        {
            _behaviour.Move(direction);
        }

        public void LookChange(Vector2 eulers)
        {
        
            _behaviour.LookChange();
            rotationBehaviour.RotateInAngles(eulers.x);
        }

        public void TouchesGround()
        {
            _behaviour.TouchesGround();
        }

        public void Jump()
        {
            _behaviour.Jump();
        }
        
        public void Update()
        {
            _behaviour.OnBehaviourUpdate();
        }

        public void FixedUpdate()
        {
            if (_shouldStop)
            {
                _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Impulse);
                _shouldStop = false;
            }
            else {
                _behaviour.OnBehaviourFixedUpdate();
            }
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        public void AccumulateForce(float addedForce)
        {
             _accumulatedForce = Mathf.Clamp(_accumulatedForce + addedForce, 0f, maxAccumulatedForce);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(feetPivot.position, Vector3.down * groundedDistance);
        }

        public float GetAccumulatedForceAndFlush()
        {
            float force = _accumulatedForce; 
            _accumulatedForce = 0f;
            return force;
        }

        public float GetMaxAccumulatedForce()
        {
            return maxAccumulatedForce;
        }
    }
}
