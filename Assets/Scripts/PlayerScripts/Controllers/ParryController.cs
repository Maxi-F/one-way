using System;
using System.Collections;
using UnityEngine;

namespace PlayerScripts.Controllers
{
    public class ParryController : MonoBehaviour
    {
        [SerializeField] private float bufferBetweenAttacksInSeconds = 0.5f;
        [SerializeField] private float timeAttackingInSeconds = 1f; 
        [SerializeField] private SphereCollider attackSphere;

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
            
            StartCoroutine(enableAttack());
        }
        
        public void Attack()
        {
            if (_canAttack)
            {
                _isAttacking = true;
                attackSphere.gameObject.SetActive(true);
                StartCoroutine(StopAttack());
            }
        }

        IEnumerator StopAttack()
        {
            yield return new WaitForSeconds(timeAttackingInSeconds);

            _isAttacking = false;
            attackSphere.gameObject.SetActive(false);
        }

        IEnumerator enableAttack()
        {
            yield return new WaitForSeconds(bufferBetweenAttacksInSeconds);

            _canAttack = true;
        }
    }
}
