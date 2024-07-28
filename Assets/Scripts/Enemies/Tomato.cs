using System;
using System.Collections;
using Audio;
using Enemies.Pools;
using Health;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Tomato : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private Vector2 secondsTillHitRange = new Vector2(0.5f, 1.0f);
        [SerializeField] private float secondsUntilDisable = 5.0f;

        [Header("Sounds")] [SerializeField] private string tomatoSplashSound = "tomatoSplash";
        
        private Rigidbody _rigidbody;
        private bool _canDamage = true;
        private Coroutine _disableTomatoCoroutine = null;
        
        public void OnEnable()
        {
            _rigidbody ??= GetComponent<Rigidbody>();

            _disableTomatoCoroutine = StartCoroutine(DisableTomato());
        }

        IEnumerator DisableTomato()
        {
            yield return new WaitForSeconds(secondsUntilDisable);
         
            StopTomatoAndReturnToPool();
        }

        private void StopTomatoAndReturnToPool()
        {
            _canDamage = true;
            _rigidbody.velocity = Vector3.zero;
            
            TomatoObjectPool.Instance.ReturnToPool(gameObject);

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _canDamage)
            {
                ITakeDamage player = other.GetComponent<ITakeDamage>();
            
                player.TakeDamage();
                
                StopCoroutine(_disableTomatoCoroutine);
                StopTomatoAndReturnToPool();
            }
        }

        /// <summary>
        /// Throws the tomato to the direction of the playerTransform,
        /// Does an oblicuous throw.
        /// </summary>
        /// <param name="playerTransform">position of the player to throw the tomato to.</param>
        public void ThrowTo(Transform playerTransform)
        {
            transform.LookAt(playerTransform.position);
            
            float secondsUntilHit = Random.Range(secondsTillHitRange.x, secondsTillHitRange.y);

            Vector3 upwardsForce = GetUpwardsForce(playerTransform, secondsUntilHit);

            Vector3 horizontalForce = GetHorizontalForce(playerTransform, secondsUntilHit);
            
            Vector3 force = upwardsForce + horizontalForce;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        /// <summary>
        /// Gets the upwards velocity that the tomato has to have at t0 to get to the player.
        /// </summary>
        /// <param name="playerTransform">position of the player to throw the tomato to.</param>
        /// <param name="secondsUntilHit">seconds until the tomato gets to the player.</param>
        /// <returns></returns>
        private Vector3 GetUpwardsForce(Transform playerTransform, float secondsUntilHit)
        {
            Vector3 direction = playerTransform.position - transform.position;
            Vector3 acceleration = (-Physics.gravity) * (0.5f * (secondsUntilHit * secondsUntilHit));
            
            Vector3 upwardsForce = (direction + acceleration) / secondsUntilHit;
            upwardsForce.x = 0;
            upwardsForce.z = 0;

            return upwardsForce;
        }

        /// <summary>
        /// Gets the horizontal velocity that the tomato has to have at t0 to get to the player.
        /// </summary>
        /// <param name="playerTransform">position of the player to throw the tomato to.</param>
        /// <param name="secondsUntilHit">seconds until the tomato gets to the player.</param>
        /// <returns></returns>
        private Vector3 GetHorizontalForce(Transform playerTransform, float secondsUntilHit)
        {
            Vector3 horizontalForce = (playerTransform.position - transform.position) / secondsUntilHit;
            horizontalForce.y = 0;

            return horizontalForce;
        }

        public void TakeDamage()
        {
            AudioManager.Instance?.PlaySound(tomatoSplashSound);
            
            _canDamage = false;

            Vector3 force = 2.0f * -(_rigidbody.velocity);
            
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
