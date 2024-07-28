using System.Collections;
using System.Collections.Generic;
using Audio;
using Health;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Enemies.EnemyFan
{
    public class EnemyFan : MonoBehaviour, IEnemy
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float disableAfterSeconds = 5.0f;
        
        [Header("Events")]
        [SerializeField] private string enemyFanEnabledEvent = "enemyEnabled";

        [Header("Sounds")] [SerializeField] private string enemyFanDeathSound = "fanDeath";
        
        [Header("Internal events")] 
        [SerializeField] private UnityEvent<bool> onShouldFollow;
        [SerializeField] private UnityEvent onDead;
        [SerializeField] private UnityEvent onReset;
        
        private Player _player;
        private Vector3 _initPosition;
        private Vector3 _deathPosition;
        private bool _shouldFollow;
        private bool _isDead;
        private CapsuleCollider _collider;
        private Rigidbody _rigidbody;
        
        void Start()
        {
            _initPosition = transform.position;
            _collider ??= GetComponent<CapsuleCollider>();
            _rigidbody ??= GetComponent<Rigidbody>();
            
            EventManager.Instance.TriggerEvent(enemyFanEnabledEvent, new Dictionary<string, object>()
            {
                { "enemy", this }
            });
        }

        void Update()
        {
            if (!_player.transform || !_shouldFollow) return;

            if (_isDead)
            {
                transform.position = _deathPosition;
                
                return;
            }

            Vector3 direction = (_player.transform.position - transform.position).normalized;

            transform.position += direction * (speed * Time.deltaTime);
            
            transform.LookAt(new Vector3(
                _player.transform.position.x,
                transform.position.y,
                _player.transform.position.z
                ));
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && !_isDead)
            {
                ITakeDamage player = other.gameObject.GetComponent<ITakeDamage>();
            
                player.TakeDamage();
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Sets the player transform.
        /// </summary>
        public void SetPlayer(Player player)
        {
            _player = player;
        }
        
        /// <summary>
        /// Resets the enemy position to initial one.
        /// </summary>
        public void ResetEnemy()
        {
            transform.position = _initPosition;

            _shouldFollow = false;
            _isDead = false;
            gameObject.SetActive(true);
            _collider.enabled = true;
            _rigidbody.useGravity = true;

            onReset?.Invoke();
        }

        /// <summary>
        /// If enemy health is zero, this method is called to start dead state.
        /// </summary>
        public void Dead()
        {
            onDead?.Invoke();
            AudioManager.Instance?.PlaySound(enemyFanDeathSound);
            
            _isDead = true;
            _collider.enabled = false;
            _rigidbody.useGravity = false;

            _deathPosition = transform.position;
            
            StartCoroutine(DisableEnemy());
        }

        /// <summary>
        /// Disables the enemy after some time.
        /// </summary>
        IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(disableAfterSeconds);
            
            if(_isDead)
                gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the follow boolean if the player is near or far from the enemy.
        /// </summary>
        public void SetFollow(bool shouldFollow)
        {
            _shouldFollow = shouldFollow;

            onShouldFollow?.Invoke(_shouldFollow);
        }
    }
}
