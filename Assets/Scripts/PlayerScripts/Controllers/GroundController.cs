using UnityEngine;

namespace PlayerScripts.Controllers
{
    public class GroundController : MonoBehaviour
    {
        [Header("Player config")]
        [SerializeField] private Transform feetPivot;
    
        [Header("Floor layer")]
        [SerializeField] private LayerMask floor;
    
        [Header("Grounded Settings")]
        [Tooltip("Raycast distance to check if player is in ground or not")]
        [SerializeField] private float groundedRaycastDistance = 0.2f;
        [Tooltip("Distance from floor that the player must be on")]
        [SerializeField] private float groundedRideDistance = 0.1f;
        
        [Tooltip("Force with which the player will be pulled to the ride distance")]
        [SerializeField] private float springForce = 5f;
        [Tooltip("Force damper from the spring, so it makes the player not bounce on the ground.")]
        [SerializeField] private float springDamper = 2.0f;
    
        private Rigidbody _rigidbody;
        private PlayerController _playerController;
    
        private bool _isOnGround;
        private RaycastHit _hit;

        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _playerController ??= GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            if (_isOnGround && _rigidbody.useGravity)
            {
                MovePlayerToGround();
            }
        }

        /// <summary>
        /// Moves player to the ground or out of the ground, mantaining certain distance between
        /// the ground and the player (it makes the player hover).
        /// </summary>
        private void MovePlayerToGround()
        {
            Vector3 velocity = _rigidbody.velocity;
            Vector3 downRayDirection = feetPivot.transform.TransformDirection(Vector3.down);

            float rayDirVelocity = Vector3.Dot(downRayDirection, velocity);
            float distance = _hit.distance - groundedRideDistance;

            float springForceToUse = (distance * springForce) - (rayDirVelocity * springDamper); 

            _rigidbody.AddForce(downRayDirection * springForceToUse, ForceMode.Force);
        }

        private void Update()
        {
            CheckPlayerOnGround();
        }

        /// <summary>
        /// Makes a raycast from feet pivot to grounded distance to check
        /// if player is in ground or not.
        /// </summary>
        private void CheckPlayerOnGround()
        {
            _isOnGround = Physics.Raycast(
                feetPivot.position, 
                Vector3.down, 
                out _hit, 
                groundedRaycastDistance, 
                floor
            ) && _playerController.JumpingBreakTime();
        }

        /// <summary>
        /// Returns if player is on ground or not.
        /// </summary>
        public bool IsOnGround()
        {
            return _isOnGround;
        }
    
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(feetPivot.position, Vector3.down * groundedRaycastDistance);
        }
    }
}
