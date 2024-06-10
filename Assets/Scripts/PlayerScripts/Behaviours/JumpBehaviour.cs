using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBehaviour : MonoBehaviour, IBehaviour
    {
        [Header("General settings")]
        [SerializeField] Transform feetPivot;
        [SerializeField] private float force;
    
        [Header("Floor layer settings")]
        [SerializeField] private LayerMask floor;
    
        [Header("Jump Settings")]
        [SerializeField] [Range(0.01f, 20f)] private float movingInAirAcceleration;
        [SerializeField] [Range(1.1f, 10f)] private float powerJumpImpulse = 2.0f;

        [Header("Player scripts")]
        [SerializeField] private WalkingBehaviour walkingBehaviour;
        [SerializeField] private Player player;

        [Header("Events")]
        [SerializeField] private UnityEvent OnLand;

        private EdgeGrabBehaviour _edgeGrabBehaviour;
        
        private RaycastHit _edgeRaycastHit;
        
        private Rigidbody _rigidBody;
        private bool _isJumping = false;
        private bool _shouldJump = false;

        private void Start()
        {
            _edgeGrabBehaviour ??= GetComponent<EdgeGrabBehaviour>();
            _rigidBody ??= GetComponent<Rigidbody>();
            walkingBehaviour ??= GetComponent<WalkingBehaviour>();
            player ??= GetComponent<Player>();
        }

        public void OnBehaviourFixedUpdate()
        {
            if (_shouldJump && CanJump())
            {
                if (player.UseAccumulativeForceOnJump)
                {
                    Vector3 upForce = (force + powerJumpImpulse) * Vector3.up;

                    Vector3 slowDownForce = -_rigidBody.velocity;
                    slowDownForce.y = 0;

                    _rigidBody.AddForce(upForce + slowDownForce, ForceMode.Impulse);
                }
                else
                {
                    _rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
                }
                _shouldJump = false;
                walkingBehaviour.ResetCoyoteTime();
                _isJumping = true;
            } else
            {
                MoveInAir();
            }
        }

        public void MoveInAir()
        {
            walkingBehaviour.MoveInAir(movingInAirAcceleration);
        }

        public void TouchesGround()
        {
        }

        public void Move(Vector3 direction)
        {
            walkingBehaviour.Move(direction);
        }

        public string GetName()
        {
            return "Jump Behaviour";
        }

        public void LookChange()
        {
            walkingBehaviour.LookChange();
        }

        public void OnBehaviourUpdate()
        {
            if (IsOnFloor() && _isJumping)
            {
                player.SetBehaviour(walkingBehaviour);
                player.TouchesGround();
                OnLand.Invoke();
                _isJumping = false;
            } else if (IsOnEdge())
            {
                player.SetBehaviour(_edgeGrabBehaviour);
                _edgeGrabBehaviour.SetEdgePosition(transform, player.GetForwardEdgeHit(), player.GetDownEdgeHit());
                _isJumping = false;
            }
        }

        public void Jump()
        {
            if (IsOnFloor() || IsOnCoyoteTimeFloor())
            {
                _shouldJump = true;
                _timeJumped = Time.time * 1000f;
            }
            else
            {
                _isJumping = true;
            }
        }

        private bool IsOnEdge()
        {
            return player.IsOnEdge();
        }

        public bool IsOnCoyoteTimeFloor()
        {
            return !walkingBehaviour.CoyoteTimePassed() && JumpingBreakTime();
        }

        public bool CanJump()
        {
            return (feetPivot && IsRaycastOnFloor()) || IsOnCoyoteTimeFloor();
        }
    }
}
