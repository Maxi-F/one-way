using System.Collections;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;

namespace Enemies.EnemyInCrowd
{
    public class EnemyCrowd : MonoBehaviour, IEnemy
    {
        [SerializeField] private List<EnemyInCrowd> enemiesFromCrowd;
        [SerializeField] private float secondsUntilDisable = 2.0f;
        [SerializeField] private float deadVelocity = 1000.0f;
        
        [Header("events")] [SerializeField] private string enemyEnabledEvent = "enemyEnabled";

        private Player _player;
        private Vector3 _startPosition;
        private Vector3 _thrownDirection;
        private bool _isBeingThrown = false;
        
        public void Start()
        {
            _startPosition = transform.position;
            
            EventManager.Instance.TriggerEvent(enemyEnabledEvent, new Dictionary<string, object>()
            {
                { "enemy", this }
            });
        }

        public void Update()
        {
            if (_isBeingThrown)
            {
                transform.position += _thrownDirection * (deadVelocity * Time.deltaTime);
            }
        }
    
        public void TakeDamage()
        {
            Dead();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            foreach (var enemyInCrowd in enemiesFromCrowd)
            {
                enemyInCrowd.SetPlayer(player);
            }
        }

        public void ResetEnemy()
        {
            StopCoroutine(DisableEnemy());
            transform.position = _startPosition;

            _isBeingThrown = false;
            gameObject.SetActive(true);
        }

        private IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(secondsUntilDisable);

            _isBeingThrown = false;
            gameObject.SetActive(false);
        }

        public void Dead()
        {
            _isBeingThrown = true;
            
            _thrownDirection = (transform.position - _player.transform.position);
            _thrownDirection.y = 0;
            _thrownDirection = _thrownDirection.normalized;
            
            StartCoroutine(DisableEnemy());
        }
    }
}
