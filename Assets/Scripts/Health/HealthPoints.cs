using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Health
{
    public class HealthPoints : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int initHitPoints = 1;
        [SerializeField] private string entity = "entity";
        
        [Header("events")]
        [SerializeField] private string entityDeadEvent = "entityDead";
        
        public int CurrentHp { get; private set; }

        void Start()
        {
            CurrentHp = initHitPoints;
        }
        
        public void TakeDamage()
        {
            Debug.Log($"taking damage... {gameObject.name}");
            CurrentHp -= 1;

            if (CurrentHp <= 0)
            {
                EventManager.Instance.TriggerEvent(entityDeadEvent, new Dictionary<string, object>()
                {
                    { "entity", entity },
                    { "gameObject", gameObject }
                });
            }
        }
    }
}
