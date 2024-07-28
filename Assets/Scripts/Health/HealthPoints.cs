using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    public class HealthPoints : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int initHitPoints = 1;
        
        [Header("events")]
        [SerializeField] private UnityEvent onEntityDead;
        
        public int CurrentHp { get; private set; }

        void Start()
        {
            CurrentHp = initHitPoints;
        }

        public void ResetHitPoints()
        {
            CurrentHp = initHitPoints;
        }
        
        public void TakeDamage()
        {
            Debug.Log($"taking damage... {gameObject.name}");
            CurrentHp -= 1;

            if (CurrentHp <= 0)
            {
                onEntityDead?.Invoke();
            }
        }
    }
}
