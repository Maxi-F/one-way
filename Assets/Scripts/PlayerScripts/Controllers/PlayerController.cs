using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float coyoteTime = 0.5f;

        [Header("Events")]
        [SerializeField] private UnityEvent OnFalling;
        [SerializeField] private UnityEvent OnEdgeJump;
        [SerializeField] private UnityEvent OnJump;
        
        private Player _player;
        private float _timePassedWithoutTouchingGround = 0f;
        private MoveController _moveController;
        private JumpController _jumpController;
        private EdgeGrabController _edgeGrabController;
        private FlyController _flyController;

        private bool _cheatsEnabled = false;
        private bool _isFalling = false;
        
        private void Start()
        {
            _isFalling = false;
            
            _player ??= GetComponent<Player>();
            _moveController ??= GetComponent<MoveController>();
            _jumpController ??= GetComponent<JumpController>();
            _edgeGrabController ??= GetComponent<EdgeGrabController>();
            _flyController ??= GetComponent<FlyController>();
        }

        public void Update()
        {
            if (!IsOnFloor() && !_edgeGrabController.IsEdgeGrabbing)
            {
                _timePassedWithoutTouchingGround += Time.deltaTime;
                if (_timePassedWithoutTouchingGround > coyoteTime)
                {
                    if (!_jumpController.IsJumping && !_cheatsEnabled)
                    {
                        _player.Jump();
                    }
                    if (!_isFalling)
                    {
                        OnFalling.Invoke();
                    }
                    _isFalling = true;
                }
            }

            if (IsOnFloor())
            {
                _isFalling = false;
            }
        }
        
        /// <summary>
        /// Resets the coyote time.
        /// </summary>
        internal void ResetCoyoteTime()
        {
            if(IsOnFloor())
                _timePassedWithoutTouchingGround = 0f;
        }

        /// <summary>
        /// Returns the jumping break time from jump controller.
        /// </summary>
        public bool JumpingBreakTime()
        {
            return _jumpController.JumpingBreakTime();
        }

        /// <summary>
        /// Returns if player is on floor or not.
        /// </summary>
        public bool IsOnFloor()
        {
            return _jumpController.IsOnFloor();
        }

        /// <summary>
        /// Returns if coyote time has already passed.
        /// </summary>
        public bool CoyoteTimePassed()
        {
            return _timePassedWithoutTouchingGround < float.Epsilon || _timePassedWithoutTouchingGround > coyoteTime;
        }

        /// <summary>
        /// Makes the player jump or go up, depending if cheats were enabled or not.
        /// </summary>
        public void Jump()
        {
            if (_cheatsEnabled)
            {
                _flyController.GoUp();
                return;
            }
            
            if(CanJump())
            {
                if (_edgeGrabController.IsEdgeGrabbing)
                {
                    OnEdgeJump?.Invoke();
                }
                else if(IsOnFloor() || IsOnCoyoteTimeFloor())
                {
                    OnJump?.Invoke();
                }
                
                _player.Jump();
            }
        }

        /// <summary>
        /// Returns if coyote time has not passed and the cooldown for a jump already passed.
        /// </summary>
        public bool IsOnCoyoteTimeFloor()
        {
            return !CoyoteTimePassed() && JumpingBreakTime();
        }

        /// <summary>
        /// Returns if player can jump or not.
        /// </summary>
        public bool CanJump()
        {
            return _jumpController.CanJump() || IsOnCoyoteTimeFloor() || _edgeGrabController.IsEdgeGrabbing;
        }

        /// <summary>
        /// Makes the player Go down (go down event).
        /// </summary>
        /// <param name="value">Checks if go down button is pressed.</param>
        public void SetFly(bool value)
        {
            _flyController.GoDown = value;
        }

        /// <summary>
        /// Sets the player movement into direction
        /// </summary>
        /// <param name="direction">Desired direction to move to.</param>
        public void Move(Vector3 direction)
        {
            _moveController.Move(direction);
        }

        /// <summary>
        /// Rotates the player's desired movement from camera rotation.
        /// </summary>
        /// <param name="eulers"></param>
        /// <param name="isController"></param>
        public void LookChange(Vector2 eulers, bool isController)
        {
            _moveController.LookChange();
        }

        /// <summary>
        /// Event that toggles cheats.
        /// </summary>
        public void OnCheatsToggle()
        {
            _cheatsEnabled = !_cheatsEnabled;
        }

        /// <summary>
        /// Adds a jump to jump controller.
        /// </summary>
        public void AddJump()
        {
            _jumpController.AddJump();
        }
    }
}
