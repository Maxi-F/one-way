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
    public class Player : MonoBehaviour
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
        private HealthPoints _health;
        
        private RotationBehaviour _rotationBehaviour;
        public float Sensibility { get; set; }
        public bool IsInGodMode { get; set; }

        public void Start()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
            _rotationBehaviour ??= GetComponent<RotationBehaviour>();
            _health ??= GetComponent<HealthPoints>();

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
        public float GetHorizontalSpeed()
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
                _health.ResetHitPoints();
                
                _isInvincible = true;
                StartCoroutine(DisableInvincibility());
            }
        }

        /// <summary>
        /// Disables the invincibility on the player.
        /// </summary>
        private IEnumerator DisableInvincibility()
        {
            yield return new WaitForSeconds(invincibleTime);

            _isInvincible = false;
        }

        /// <summary>
        /// Sets attack in idle state.
        /// </summary>
        public void SetAttackInIdle()
        {
            _attackFsm.ChangeStateTo(AttackBehaviour.Idle);
        }

        /// <summary>
        /// Sets attack in Attack state.
        /// </summary>
        public void Attack()
        {
            _attackFsm.ChangeStateTo(AttackBehaviour.Attack);
        }

        /// <summary>
        /// Checks if attack FSM is in attack state.
        /// </summary>
        public bool IsAttacking()
        {
            return _attackFsm.IsCurrentBehaviour(AttackBehaviour.Attack);
        }

        /// <summary>
        /// If health is zero (because of enemies), this event is called and triggers the enemy hit event.
        /// </summary>
        public void EnemyHit()
        {
            EventManager.Instance.TriggerEvent(enemyHitEvent, null);
        }
    }
}
