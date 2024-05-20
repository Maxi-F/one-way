using System;
using UnityEngine;
using UnityEngine.Events;
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

        [Header("Events")] [SerializeField] private UnityEvent onWinboxCollided;
        
        [Header("Edge Grabbing Settings")]
        [SerializeField] private float edgeGrabUpLineStartDistance = 1.5f;
        [SerializeField] private float edgeGrabUpLineEndDistance = 0.5f;
        [SerializeField] private float edgeGrabYdistance = 0.1f;
        public bool UseAccumulativeForceOnJump { get; set; }

        public float velocity { get { return _rigidbody.velocity.magnitude; } }
        
        private Rigidbody _rigidbody;
        private bool _shouldStop = false;
        private float _accumulatedForce = 0f;

        private Vector3 _edgeLineCastStart;
        private Vector3 _edgeLineCastEnd;
        
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

        public bool CanJump()
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
            
            Gizmos.DrawLine(_edgeLineCastStart, _edgeLineCastEnd);
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

        public bool IsOnEdge()
        {
            RaycastHit upHit;
            _edgeLineCastStart = transform.position + transform.up * edgeGrabUpLineStartDistance + transform.forward;
            _edgeLineCastEnd = transform.position + transform.up * edgeGrabUpLineEndDistance + transform.forward;

            if (_rigidbody.velocity.y < 0 && Physics.Linecast(_edgeLineCastStart, _edgeLineCastEnd, out upHit, floor))
            {
                Vector3 forwardCastStart = new Vector3(transform.position.x, upHit.point.y - edgeGrabYdistance,
                    transform.position.z);
                Vector3 forwardCastEnd = forwardCastStart + transform.forward;

                return Physics.Linecast(forwardCastStart, forwardCastEnd, out var hit, floor);
            };
            return false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("WinBox"))
            {
                onWinboxCollided.Invoke();
            }
        }
    }
}
