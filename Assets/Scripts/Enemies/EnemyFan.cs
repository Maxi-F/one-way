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
        
        private Transform _playerTransform;
        private Vector3 _initPosition;
        
        void Start()
        {
            _initPosition = transform.position;
            
            EventManager.Instance.TriggerEvent(enemyFanEnabledEvent, new Dictionary<string, object>()
            {
                { "enemy", this }
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (!_playerTransform) return;

            Vector3 direction = (_playerTransform.position - transform.position).normalized;

            transform.position += direction * (speed * Time.deltaTime);
            transform.LookAt(_playerTransform.position);
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
            _playerTransform = player.transform;
        }
        
        /// <summary>
        /// Resets the enemy position to initial one.
        /// </summary>
        public void Reset()
        {
            transform.position = _initPosition;
        }
    }
}
