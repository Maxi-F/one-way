using System;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;

namespace Enemies
{
    public class EnemyFan : MonoBehaviour, IEnemy
    {
        [SerializeField] private float speed = 10.0f;
        
        [Header("Events")]
        [SerializeField] private string enemyFanEnabledEvent = "enemyFanEnabled";
        [SerializeField] private string enemyHitEvent = "enemyHit";
        
        private Player _player;
        private Vector3 _initPosition;
        private bool _shouldFollow;
        
        void Start()
        {
            _initPosition = transform.position;
            
            EventManager.Instance.TriggerEvent(enemyFanEnabledEvent, new Dictionary<string, object>()
            {
                { "enemy", this }
            });
        }

        void Update()
        {
            if (!_player.transform || !_shouldFollow) return;

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
            if (other.gameObject.CompareTag("Player"))
            {
                EventManager.Instance.TriggerEvent(enemyHitEvent, null);
                
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
        public void Reset()
        {
            transform.position = _initPosition;
            gameObject.SetActive(true);
        }

        public void SetFollow(bool shouldFollow)
        {
            _shouldFollow = shouldFollow;
        }
    }
}
