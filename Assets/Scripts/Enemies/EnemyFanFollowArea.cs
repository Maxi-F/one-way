using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyFanFollowArea : MonoBehaviour
    {
        private EnemyFan _enemyFan;
        private SphereCollider _sphereCollider;
        void Start()
        {
            _sphereCollider ??= GetComponent<SphereCollider>();
            _enemyFan ??= GetComponentInParent<EnemyFan>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _enemyFan.SetFollow(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _enemyFan.SetFollow(false);
            }
        }

        private void OnDrawGizmos()
        {
            _sphereCollider ??= GetComponent<SphereCollider>();
            Gizmos.DrawWireSphere(
                _sphereCollider.transform.position,
                _sphereCollider.radius
                );
        }
    }
}
