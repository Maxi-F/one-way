using System;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class EnemyTriggerArea : MonoBehaviour
    {
        private SphereCollider _sphereCollider;

        [Header("Internal Events")] [SerializeField]
        private UnityEvent<bool> onPlayerTrigger;
        
        void Start()
        {
            _sphereCollider ??= GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerTrigger?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerTrigger?.Invoke(false);
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
