using System.Collections;
using System.Collections.Generic;
using Enemies.Pools;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Enemies.EnemyInCrowd
{
    public class EnemyInCrowd : MonoBehaviour
    {
        [SerializeField] private Vector2 secondsToThrowRange;
        [SerializeField] private float animationWaitSeconds = 1.0f;

        [Header("Internal events")] 
        [SerializeField] private UnityEvent onThrowing;

        [SerializeField] private UnityEvent onThrown;
        
        private Player _player;
        private bool _isPlayerInArea;
        private bool _isThrowing;
    
        public void Update()
        {
            if (_isPlayerInArea)
            {
                Vector3 playerHorizontalPosition = _player.transform.position;
                playerHorizontalPosition.y = transform.position.y;
                
                transform.LookAt(playerHorizontalPosition);
                
                if(!_isThrowing)
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
            _isThrowing = true;
            onThrowing?.Invoke();
            
            yield return new WaitForSeconds(animationWaitSeconds);
            
            ThrowTomato();

            onThrown?.Invoke();
            yield return new WaitForSeconds(waitTime);

            _isThrowing = false;
        }

        private void ThrowTomato()
        {
            GameObject tomatoObject = TomatoObjectPool.Instance.GetPooledObject();

            tomatoObject.transform.position = transform.position;
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
    }
}
