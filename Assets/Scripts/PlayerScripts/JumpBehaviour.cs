using UnityEngine;

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
        [SerializeField] private float jumpingMiliseconds = 100f;
    
        [Header("Player scripts")]
        [SerializeField] private WalkingBehaviour walkingBehaviour;
        [SerializeField] private Player player;
    
        [Header("Edge Grabbing Settings")]
        [SerializeField] private float edgeGrabUpLineStartDistance = 1.5f;
        [SerializeField] private float edgeGrabUpLineEndDistance = 0.5f;
        [SerializeField] private float edgeGrabYdistance = 0.1f;

        private EdgeGrabBehaviour _edgeGrabBehaviour;
        
        private RaycastHit _edgeRaycastHit;
        
        private Rigidbody _rigidBody;
        private bool _isJumping = false;
        private bool _shouldJump = false;
        private float _timeJumped = 0f;

        private Vector3 _edgeLineCastStart;
        private Vector3 _edgeLineCastEnd;

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
                _rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
                _shouldJump = false;
                _isJumping = true;
            }
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
                _isJumping = false;
            } else if (IsOnEdge())
            {
                player.SetBehaviour(_edgeGrabBehaviour);
                _edgeGrabBehaviour.SetEdgePosition(transform);
                _isJumping = false;
            }
        }

        public void Jump()
        {
            if (IsOnFloor())
            {
                Debug.Log($"{name}: Jump!");
                _shouldJump = true;
                _timeJumped = Time.time * 1000f;
            } 
        }

        private bool JumpingBreakTime()
        {
            return _timeJumped + jumpingMiliseconds < (Time.time * 1000f);
        }

        private bool IsRaycastOnFloor()
        {
            return player.IsRaycastOnFloor();
        }

        private bool IsOnEdge()
        {
            RaycastHit upHit;
            _edgeLineCastStart = transform.position + transform.up * edgeGrabUpLineStartDistance + transform.forward;
            _edgeLineCastEnd = transform.position + transform.up * edgeGrabUpLineEndDistance + transform.forward;

            if (_rigidBody.velocity.y < 0 && Physics.Linecast(_edgeLineCastStart, _edgeLineCastEnd, out upHit, floor))
            {
                Vector3 forwardCastStart = new Vector3(transform.position.x, upHit.point.y - edgeGrabYdistance,
                    transform.position.z);
                Vector3 forwardCastEnd = forwardCastStart + transform.forward;

                return Physics.Linecast(forwardCastStart, forwardCastEnd, out _edgeRaycastHit, floor);
            };
            return false;
        }

        public bool IsOnFloor()
        {
            return IsRaycastOnFloor() && JumpingBreakTime();
        }

        public bool CanJump()
        {
            return feetPivot && IsRaycastOnFloor();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_edgeLineCastStart, _edgeLineCastEnd);
        }
    }
}
