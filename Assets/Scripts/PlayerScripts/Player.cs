using System;
using System.Collections;
using Audio;
using Health;
using Manager;
using PlayerScripts.AttackBehaviours;
using PlayerScripts.Behaviours;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, ITakeDamage
    {
        [Header("Behaviours")]
        [Header("PlayerData")]
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] WalkingBehaviour walkBehaviour;
        [SerializeField] private IdleBehaviour idleBehaviour;
        [SerializeField] private int lives = 3;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private int velocityToRun = 10;
        [SerializeField] private float invincibleTime = 3.0f;
        
        [Header("Accumulated force settings")] 
        [SerializeField] private float maxAccumulatedForce;
        
        [Header("Sounds")]
        [SerializeField] private string lostLiveSound = "lostLife";

        [Header("Events")]
        [SerializeField] private string lostLiveEvent = "lostLive";
        [SerializeField] private string enemyHitEvent = "enemyHit";
        
        public bool IsFlying { get; set; }

        public int VelocityToRun
        {
            get { return velocityToRun; }
        }

        public int Lives
        {
            get { return lives; }
        }

        public bool IsInvincible
        {
            get {
                return _isInvincible;
            }
        }
        
        private Rigidbody _rigidBody;
        private bool _shouldStop = false;
        private bool _isInvincible = false;
        private MovementFsm _movementFsm;
        private AttackFsm _attackFsm;
        
        private RotationBehaviour _rotationBehaviour;
        public float Sensibility { get; set; }
        public bool IsInGodMode { get; set; }
        public void Start()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
            _rotationBehaviour ??= GetComponent<RotationBehaviour>();

            IBehaviour[] behaviours = GetComponents<IBehaviour>();
            _movementFsm = new MovementFsm(behaviours, walkBehaviour);

            IAttackBehaviour[] attackBehaviours = GetComponents<IAttackBehaviour>();
            _attackFsm = new AttackFsm(attackBehaviours, idleBehaviour);
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
            _movementFsm.ChangeStateTo(MovementBehaviour.Move);
        }

        /// <summary>
        /// Makes the player change its state to jump.
        /// </summary>
        public void Jump()
        {
            _movementFsm.ChangeStateTo(MovementBehaviour.Jump);
        }
        
        public void Update()
        {
            _movementFsm.OnUpdate();
            _attackFsm.OnUpdate();
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
                _movementFsm.OnFixedUpdate();
                _attackFsm.OnFixedUpdate();
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
            return _movementFsm.IsCurrentBehaviour(MovementBehaviour.EdgeGrab);
        }

        
        /// <summary>
        /// Makes the player edge grab if it was not edge grabbing before.
        /// </summary>
        public void EdgeGrab()
        {
            if (!IsEdgeGrabbing())
            {
                _movementFsm.ChangeStateTo(MovementBehaviour.EdgeGrab);
            }
        }

        /// <summary>
        /// Changes the player state to fly.
        /// </summary>
        public void Fly()
        {
            _movementFsm.ChangeStateTo(MovementBehaviour.Fly);
        }

        /// <summary>
        /// makes a player lose a life
        /// </summary>
        public void LoseLive(bool fromEnemy)
        {
            if (!IsInGodMode && (!fromEnemy || !_isInvincible))
            {
                AudioManager.Instance?.PlaySound(lostLiveSound);
                EventManager.Instance?.TriggerEvent(lostLiveEvent, null);
                
                lives--;
                _isInvincible = true;
                StartCoroutine(DisableInvincibility());
            }
        }

        private IEnumerator DisableInvincibility()
        {
            yield return new WaitForSeconds(invincibleTime);

            _isInvincible = false;
        }

        public void SetAttackInIdle()
        {
            _attackFsm.ChangeStateTo(AttackBehaviour.Idle);
        }

        public void Attack()
        {
            _attackFsm.ChangeStateTo(AttackBehaviour.Attack);
        }

        public bool IsAttacking()
        {
            return _attackFsm.IsCurrentBehaviour(AttackBehaviour.Attack);
        }

        public void TakeDamage()
        {
            EventManager.Instance.TriggerEvent(enemyHitEvent, null);
        }
    }
}
