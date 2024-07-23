using System.Collections;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Player player;
    
    [Header("events")]
    [SerializeField] private string enemyFanEnabledEvent = "enemyFanEnabled";

    void OnEnable()
    {
        EventManager.Instance.SubscribeTo(enemyFanEnabledEvent, HandleNewEnemyFanEvent);    
    }

    /// <summary>
    /// When an enemy is created, this function gets called and sets
    /// the player to the enemy.
    /// </summary>
    void HandleNewEnemyFanEvent(Dictionary<string, object> message)
    {
        EnemyFanMovement enemyFan = (EnemyFanMovement)message["enemyFan"];
        
        enemyFan.SetPlayerTransform(player.transform);
    }
}
