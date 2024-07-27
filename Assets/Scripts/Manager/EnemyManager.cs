using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        private List<IEnemy> _enemies;
    
        [FormerlySerializedAs("enemyFanEnabledEvent")]
        [Header("events")]
        [SerializeField] private string enemyEnabledEvent = "enemyEnabled";
        [SerializeField] private string playerDeathEvent = "playerDeath";
    
        void OnEnable()
        {
            _enemies = new List<IEnemy>();
            EventManager.Instance.SubscribeTo(enemyEnabledEvent, HandleNewEnemyEvent);
            EventManager.Instance.SubscribeTo(playerDeathEvent, HandleResetEnemies);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnsubscribeTo(enemyEnabledEvent, HandleNewEnemyEvent);
            EventManager.Instance.UnsubscribeTo(playerDeathEvent, HandleResetEnemies);
        }

        /// <summary>
        /// When an enemy is created, this function gets called and sets
        /// the player to the enemy.
        /// </summary>
        void HandleNewEnemyEvent(Dictionary<string, object> message)
        {
            IEnemy enemy = (IEnemy)message["enemy"];
            
            enemy.SetPlayer(player);
            _enemies.Add(enemy);
        }

        /// <summary>
        /// Resets all enemies
        /// </summary>
        void HandleResetEnemies(Dictionary<string, object> message)
        {
            foreach (var enemy in _enemies.ToList())
            {
                Debug.Log("Enemy is not null. resetting...");
                enemy.ResetEnemy();
            }
        }
    }
}
