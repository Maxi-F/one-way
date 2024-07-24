using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlayerScripts.Controllers
{
    public class ParryController : MonoBehaviour
    {
        [SerializeField] private float bufferBetweenAttacksInSeconds = 0.5f;
        [SerializeField] private float timeAttackingInSeconds = 1f; 
        [SerializeField] private SphereCollider attackSphere;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent onAttack;
        [SerializeField] private UnityEvent onAttackRelease;
        
        private Player _player;
        private bool _isAttacking;
        private bool _canAttack = true;

        void Start()
        {
            _player ??= GetComponent<Player>();
        }
        
        public void OnUpdate()
        {
            if (_isAttacking) return;
            
            _player.SetAttackInIdle();
        }

        public void SetBuffer()
        {
            _canAttack = false;
            
            StartCoroutine(EnableAttack());
        }
        
        public void Attack()
        {
            if (_canAttack)
            {
                _isAttacking = true;
                attackSphere.gameObject.SetActive(true);
                StartCoroutine(StopAttack());
                
                onAttack?.Invoke();
            }
        }

        IEnumerator StopAttack()
        {
            yield return new WaitForSeconds(timeAttackingInSeconds);

            _isAttacking = false;
            onAttackRelease?.Invoke();
            attackSphere.gameObject.SetActive(false);
        }

        IEnumerator EnableAttack()
        {
            yield return new WaitForSeconds(bufferBetweenAttacksInSeconds);

            _canAttack = true;
        }
    }
}
