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
    [SerializeField] private string enemyDeadEvent = "enemyDead";
    
    void OnEnable()
    {
        _enemies = new List<IEnemy>();
        EventManager.Instance.SubscribeTo(enemyFanEnabledEvent, HandleNewEnemyFanEvent);
        EventManager.Instance.SubscribeTo(playerDeathEvent, HandleResetEnemies);
        EventManager.Instance.SubscribeTo(enemyDeadEvent, HandleEnemyDead);
    }

    /// <summary>
    /// When an enemy is created, this function gets called and sets
    /// the player to the enemy.
    /// </summary>
    void HandleNewEnemyFanEvent(Dictionary<string, object> message)
    {
        IEnemy enemy = (IEnemy)message["enemy"];

        Debug.Log("New enemy Fan!");
        
        enemy.SetPlayer(player);
        _enemies.Add(enemy);
    }

    /// <summary>
    /// Handles enemy dead event after attack.
    /// </summary>
    void HandleEnemyDead(Dictionary<string, object> message)
    {
        GameObject healthPointsObject = (GameObject)message["gameObject"];

        if ((string)message["entity"] == "enemy")
        {
            healthPointsObject.SetActive(false);
        }
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
