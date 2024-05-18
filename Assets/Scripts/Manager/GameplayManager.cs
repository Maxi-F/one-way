using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private Vector3 _startingPosition;
    
    void Start()
    {
        _startingPosition = player.transform.position;
    }
    
    public void HandleDeath()
    {
        Debug.Log("dead!");

        player.transform.position = _startingPosition;
        player.Stop();
    }
}
