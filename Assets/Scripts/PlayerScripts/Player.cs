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
        [Header("PlayerData")]
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private LayerMask floor;
        [SerializeField] Transform feetPivot;
        
        [Header("Jump Settings")]
        [SerializeField] private float groundedDistance = 0.1f;

        [SerializeField] private float sphereCastRadius = 4.0f;
        
        [Header("Accumulated force settings")] 
        [SerializeField] private float maxAccumulatedForce;

        [Header("Events")] [SerializeField] private UnityEvent onWinboxCollided;
        
        [Header("Edge Grabbing Settings")]
        [SerializeField] private float edgeGrabUpLineStartDistance = 1.5f;
        [SerializeField] private float edgeGrabUpLineEndDistance = 0.5f;
        [SerializeField] private float edgeGrabYdistance = 0.1f;
        public bool UseAccumulativeForceOnJump { get; set; }

        public float velocity { get { return _rigidbody.velocity.magnitude; } }
        public bool isFlying { get; set; }

        private Rigidbody _rigidbody;
        private bool _shouldStop = false;

        private Vector3 _edgeLineCastStart;
        private Vector3 _edgeLineCastEnd;
        private RaycastHit _edgeForwardHit;
        private RaycastHit _edgeDownHit;
        
        private RotationBehaviour _rotationBehaviour;
        public float Sensibility { get; set; }
        
        public void Start()
        {
            _behaviour ??= GetComponent<WalkingBehaviour>();
            _rigidbody ??= GetComponent<Rigidbody>();
            _rotationBehaviour ??= GetComponent<RotationBehaviour>();
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

        public void SetBehaviour(IBehaviour newBehaviour)
        {
            _behaviour = newBehaviour;
        }

        public bool CanJump()
        {
            return Physics.SphereCast(
                feetPivot.position,
                sphereCastRadius,
                Vector3.down,
                out var hit,
                groundedDistance,
                floor);
        }
        
        public void Move(Vector3 direction)
        {
            _behaviour.Move(direction);
        }

        public void LookChange(Vector2 eulers)
        {
            _behaviour.LookChange();
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
                _behaviour.OnBehaviourFixedUpdate();
            }
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(feetPivot.position + groundedDistance * Vector3.down, sphereCastRadius);
            
            Gizmos.DrawLine(_edgeLineCastStart, _edgeLineCastEnd);
        }

        public float GetMaxAccumulatedForce()
        {
            return maxAccumulatedForce;
        }

        public bool IsEdgeGrabbing()
        {
            return _behaviour.GetName() == "Edge Grab Behaviour";
        }

        public bool IsOnEdge()
        {
            _edgeLineCastStart = transform.position + transform.up * edgeGrabUpLineStartDistance + transform.forward;
            _edgeLineCastEnd = transform.position + transform.up * edgeGrabUpLineEndDistance + transform.forward;

            if (_rigidbody.velocity.y < 0 && Physics.Linecast(_edgeLineCastStart, _edgeLineCastEnd, out _edgeDownHit, floor))
            {
                Vector3 forwardCastStart = new Vector3(transform.position.x, _edgeDownHit.point.y - edgeGrabYdistance,
                    transform.position.z);
                Vector3 forwardCastEnd = forwardCastStart + transform.forward;

                return Physics.Linecast(forwardCastStart, forwardCastEnd, out _edgeForwardHit, floor);
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

        public void SetGravity(bool active)
        {
            _rigidbody.useGravity = active;
        }

        public RaycastHit GetForwardEdgeHit()
        {
            return _edgeForwardHit;
        }

        public RaycastHit GetDownEdgeHit()
        {
            return _edgeDownHit;
        }
    }
}
