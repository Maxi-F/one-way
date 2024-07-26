using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.Pools;
using Manager;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCrowd : MonoBehaviour, IEnemy
{
    [SerializeField] private Vector2 secondsToThrowRange;

    [Header("events")] [SerializeField] private string enemyEnabledEvent = "enemyEnabled";
    
    private Player _player;
    private bool _isPlayerInArea;
    private bool _isThrowing;

    public void Start()
    {
        EventManager.Instance.TriggerEvent(enemyEnabledEvent, new Dictionary<string, object>()
        {
            { "enemy", this }
        });
    }
    
    public void Update()
    {
        if (_isPlayerInArea && !_isThrowing)
        {
            StartCoroutine(
                Throw(
                Random.Range(secondsToThrowRange.x, secondsToThrowRange.y)
                )
            );
        }
    }

    IEnumerator Throw(float waitTime)
    {
        ThrowTomato();
        
        _isThrowing = true;

        yield return new WaitForSeconds(waitTime);

        _isThrowing = false;
    }

    private void ThrowTomato()
    {
        GameObject tomatoObject = TomatoObjectPool.Instance.GetPooledObject();

        tomatoObject.SetActive(true);
        Tomato tomato = tomatoObject.GetComponent<Tomato>();
        
        tomato.ThrowTo(_player.transform);
    }
    
    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void SetPlayer(Player player)
    {
        Debug.Log("player?");
        _player = player;
    }

    public void SetPlayerInArea(bool isPlayerInArea)
    {
        _isPlayerInArea = isPlayerInArea;
    }
    
    public void ResetEnemy()
    {
        throw new System.NotImplementedException();
    }

    public void Dead()
    {
        throw new System.NotImplementedException();
    }
}
