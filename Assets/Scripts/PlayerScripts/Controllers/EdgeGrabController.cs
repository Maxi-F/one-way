using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts.Controllers
{
    public class EdgeGrabController : MonoBehaviour
    {
        [SerializeField] private LayerMask floor;
    
        [Header("Edge Grabbing Settings")]
        [Tooltip("Start of the line that checks for edge grabs.")] [SerializeField] private float edgeGrabUpLineStartDistance = 1.5f;
        [Tooltip("End of the line that checks for edge grabs.")] [SerializeField] private float edgeGrabUpLineEndDistance = 0.5f;
        [Tooltip("Y Distance relative to the forward line cast of the edge grab.")] [SerializeField] private float edgeGrabYdistance = 0.1f;
        
        [SerializeField] private float powerJumpImpulse;
        [SerializeField] private Vector2 edgeOffset = new Vector2(0.1f, 1);
        [SerializeField] private float secondsUntilEdgeGrabAvailable = 2.0f;
    
        [Header("Events")]
        [SerializeField] private UnityEvent OnEdgePositionSetted;
    
        private Vector3 _edgeLineCastStart;
        private Vector3 _edgeLineCastEnd;
        private RaycastHit _edgeForwardHit;
        private RaycastHit _edgeDownHit;

        private Rigidbody _rigidBody;
        private Player _player;

        private float _timeUntilEdgeGrabAvailable = 0.0f;
        private Vector3 _edgePosition;
        private Vector3 _edgeNormal;
        public bool IsEdgeGrabbing { get; set; }

        void Start()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
            _player ??= GetComponent<Player>();
        }
    
        /// <summary>
        /// Checks if the player is on an edge of a platform.
        /// </summary>
        /// <returns>bool that indicates that the player is on an edge or not.</returns>
        public bool IsOnEdge()
        {
            if (!IsEdgeGrabAvailable()) return false;
        
            _edgeLineCastStart = transform.position + transform.up * edgeGrabUpLineStartDistance + transform.forward;
            _edgeLineCastEnd = transform.position + transform.up * edgeGrabUpLineEndDistance + transform.forward;

            if (_rigidBody.velocity.y < 0 && Physics.Linecast(_edgeLineCastStart, _edgeLineCastEnd, out _edgeDownHit, floor))
            {
                Vector3 forwardCastStart = new Vector3(transform.position.x, _edgeDownHit.point.y - edgeGrabYdistance,
                    transform.position.z);
                Vector3 forwardCastEnd = forwardCastStart + transform.forward;

                return Physics.Linecast(forwardCastStart, forwardCastEnd, out _edgeForwardHit, floor);
            };
            return false;
        }
    
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            
            Gizmos.DrawLine(_edgeLineCastStart, _edgeLineCastEnd);
        }

        /// <summary>
        /// Checks if a player is on edge.
        /// if it is, it makes the player edge grab (Goes into new edge grab state).
        /// </summary>
        public void CheckEdge()
        {
            if (IsOnEdge())
            {
                _player.EdgeGrab();
                OnEdgePositionSetted.Invoke();
            }
        }
    
        /// <summary>
        /// Makes the player stay in the place of the edge position.
        /// </summary>
        public void StayInPlace()
        {
            transform.position = _edgePosition;
            transform.forward = -_edgeNormal;
        }
    
        /// <summary>
        /// Makes the rigid body not use gravity so it edge grabs.
        /// </summary>
        public void EdgeGrab()
        {
            _rigidBody.useGravity = false;
        }
    
        /// <summary>
        /// Sets the edge position for the player to hang.
        /// </summary>
        public void SetEdgePosition()
        {
            _rigidBody.AddForce(-_rigidBody.velocity, ForceMode.Impulse);

            Vector3 hangPosition = new Vector3(_edgeForwardHit.point.x, _edgeDownHit.point.y, _edgeForwardHit.point.z);
            Vector3 offset = transform.forward * -edgeOffset.x + transform.up * -edgeOffset.y;
            hangPosition += offset;
        
            _edgePosition = hangPosition;
            _edgeNormal = new Vector3(_edgeForwardHit.normal.x, 0, _edgeForwardHit.normal.z);
        }

        /// <summary>
        /// Checks if the edge grab is available or not (Edge grab cooldown)
        /// </summary>
        private bool IsEdgeGrabAvailable()
        {
            return _timeUntilEdgeGrabAvailable < Time.time;
        } 
        
        /// <summary>
        /// Sets the edge grab timer cooldown.
        /// </summary>
        public void SetEdgeGrabTimer()
        {
            _timeUntilEdgeGrabAvailable = Time.time + secondsUntilEdgeGrabAvailable;
            _rigidBody.useGravity = true;
        }
    }
}
