using System.Collections;
using System.Collections.Generic;
using Audio;
using Health;
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

        [Header("Sounds")] [SerializeField] private string enemyDeathSound = "crowdDeath";
        
        private Player _player;
        private Vector3 _startPosition;
        private Vector3 _thrownDirection;
        private bool _isBeingThrown = false;
        private Coroutine _disableEnemyCoroutine;
        
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
            if(_disableEnemyCoroutine != null)
                StopCoroutine(_disableEnemyCoroutine);
            transform.position = _startPosition;

            foreach (var enemyInCrowd in enemiesFromCrowd)
            {
                enemyInCrowd.ResetEnemy();
            }
            
            _isBeingThrown = false;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Disables the enemy crowd after some time has passed, so it does not
        /// get thrown infinitely.
        /// </summary>
        private IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(secondsUntilDisable);

            _isBeingThrown = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Makes the enemy crowd "die", throwing them into infinity.
        /// </summary>
        public void Dead()
        {
            _isBeingThrown = true;
            AudioManager.Instance?.PlaySound(enemyDeathSound);
            
            foreach (var enemyInCrowd in enemiesFromCrowd)
            {
                enemyInCrowd.StopThrowing();
            }
            
            _thrownDirection = (transform.position - _player.transform.position);
            _thrownDirection.y = 0;
            _thrownDirection = _thrownDirection.normalized;
            
            _disableEnemyCoroutine = StartCoroutine(DisableEnemy());
        }
    }
}
