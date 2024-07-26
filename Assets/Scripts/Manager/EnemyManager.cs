using System.Collections;
using System.Collections.Generic;
using Enemies;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

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

    /// <summary>
    /// When an enemy is created, this function gets called and sets
    /// the player to the enemy.
    /// </summary>
    void HandleNewEnemyEvent(Dictionary<string, object> message)
    {
        IEnemy enemy = (IEnemy)message["enemy"];

        Debug.Log("New enemy!");
        
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
            enemy.ResetEnemy();
        }
    }
}
