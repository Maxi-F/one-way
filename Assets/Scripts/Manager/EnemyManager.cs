using System.Collections;
using System.Collections.Generic;
using Enemies;
using Manager;
using PlayerScripts;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<IEnemy> _enemies;
    
    [Header("events")]
    [SerializeField] private string enemyFanEnabledEvent = "enemyFanEnabled";
    [SerializeField] private string playerDeathEvent = "playerDeath";

    void OnEnable()
    {
        _enemies = new List<IEnemy>();
        EventManager.Instance.SubscribeTo(enemyFanEnabledEvent, HandleNewEnemyFanEvent);
        EventManager.Instance.SubscribeTo(playerDeathEvent, HandleResetEnemies);
    }

    /// <summary>
    /// When an enemy is created, this function gets called and sets
    /// the player to the enemy.
    /// </summary>
    void HandleNewEnemyFanEvent(Dictionary<string, object> message)
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
        foreach (var enemy in _enemies)
        {
            enemy.Reset();
        }
    }
}
