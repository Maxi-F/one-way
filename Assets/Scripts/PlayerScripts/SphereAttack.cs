using System;
using Enemies;
using Health;
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
                ITakeDamage enemy = other.gameObject.GetComponent<ITakeDamage>();
            
                enemy?.TakeDamage();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        }
    }
}
