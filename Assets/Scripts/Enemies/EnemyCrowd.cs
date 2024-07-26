using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCrowd : MonoBehaviour, IEnemy
{
    [SerializeField] private Vector2 secondsToThrowRange;
    
    private Player _player;
    private bool _isPlayerInArea;
    private bool _isThrowing;
    
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
        // throw
        _isThrowing = true;

        yield return new WaitForSeconds(waitTime);

        _isThrowing = false;
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void SetPlayer(Player player)
    {
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
