using System;
using Enemies;
using UnityEngine;

namespace PlayerScripts
{
    public class SphereAttack : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            
                enemy.TakeDamage();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        }
    }
}
