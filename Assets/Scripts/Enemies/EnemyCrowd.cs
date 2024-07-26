using System.Collections;
using System.Collections.Generic;
using Enemies.Pools;
using Manager;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyCrowd : MonoBehaviour, IEnemy
    {
        [SerializeField] private Vector2 secondsToThrowRange;

        [Header("events")] [SerializeField] private string enemyEnabledEvent = "enemyEnabled";
    
        private Player _player;
        private bool _isPlayerInArea;
        private bool _isThrowing;

        public void Start()
        {
            EventManager.Instance.TriggerEvent(enemyEnabledEvent, new Dictionary<string, object>()
            {
                { "enemy", this }
            });
        }
    
        public void Update()
        {
            if (_isPlayerInArea && !_isThrowing)
            {
                StartCoroutine(
                    Throw(
                        Random.Range(secondsToThrowRange.x, secondsToThrowRange.y)
                    )
                );
            }
        }

        /// <summary>
        /// Coroutine that throws the tomato to the player.
        /// </summary>
        IEnumerator Throw(float waitTime)
        {
            ThrowTomato();
        
            _isThrowing = true;

            yield return new WaitForSeconds(waitTime);

            _isThrowing = false;
        }

        private void ThrowTomato()
        {
            GameObject tomatoObject = TomatoObjectPool.Instance.GetPooledObject();

            tomatoObject.transform.position = transform.position;
            tomatoObject.transform.SetParent(transform);
            tomatoObject.SetActive(true);
        
            Tomato tomato = tomatoObject.GetComponent<Tomato>();
        
            tomato.ThrowTo(_player.transform);
        }
    
        public void TakeDamage()
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public void SetPlayerInArea(bool isPlayerInArea)
        {
            _isPlayerInArea = isPlayerInArea;
        }
    
        public void ResetEnemy()
        {
            _isPlayerInArea = false;
        }

        public void Dead()
        {
        }
    }
}
