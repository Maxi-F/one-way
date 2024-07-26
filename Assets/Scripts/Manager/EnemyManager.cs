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
            Debug.Log("On enable!");
            _enemies = new List<IEnemy>();
            EventManager.Instance.SubscribeTo(enemyEnabledEvent, HandleNewEnemyEvent);
            EventManager.Instance.SubscribeTo(playerDeathEvent, HandleResetEnemies);
        }

        /// <summary>
        /// When an enemy is created, this function gets called and sets
        /// the player to the enemy.
        /// </summary>
        void HandleNewEnemyEvent(Dictionary<string, object> message)
        {
            IEnemy enemy = (IEnemy)message["enemy"];
            
            if (enemy == null)
            {
                return;
            }
            
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
                if (enemy.HasBeenDestroyed()) // Could happen because of scene changes that enemyManager has enemies from other level.
                {
                    Debug.Log("Enemy is null. removing...");
                    _enemies.Remove(null);
                    continue;
                }
                
                Debug.Log("Enemy is not null. resetting...");
                enemy.ResetEnemy();
            }
        }
    }
}
