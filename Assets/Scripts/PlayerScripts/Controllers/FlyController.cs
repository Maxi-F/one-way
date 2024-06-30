using UnityEngine;

namespace PlayerScripts.Controllers
{
    public class FlyController : MonoBehaviour
    {
        [SerializeField] private float velocity;
    
        private MoveController _moveController; 
        
        private bool _isGoingUp = false;
        private Rigidbody _rigidbody;
    
        public bool GoDown { get; set; }

        public void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _moveController ??= GetComponent<MoveController>();
        }
    
        /// <summary>
        /// Makes the player start flying, setting gravity to false.
        /// </summary>
        public void StartFly()
        {
            _rigidbody.useGravity = false;
            _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        }

        /// <summary>
        /// Makes the player stop flying, reactivating gravity.
        /// </summary>
        public void EndFly()
        {
            _rigidbody.useGravity = true;
        }

        /// <summary>
        /// makes the player go up or stop going up.
        /// </summary>
        public void GoUp()
        {
            _isGoingUp = !_isGoingUp;
        }

        /// <summary>
        /// Fly update behaviour.
        /// </summary>
        public void Fly()
        {
            Vector3 upVector = _isGoingUp ? Vector3.up : Vector3.zero;
            Vector3 downVector = GoDown ? Vector3.down : Vector3.zero;
        
            transform.position += (_moveController.Direction + upVector + downVector) * (velocity * Time.deltaTime);
        }
    }
}
