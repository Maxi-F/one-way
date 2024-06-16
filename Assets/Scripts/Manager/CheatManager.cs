using System.Collections;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

public class CheatManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Player player;
    [SerializeField] private FlyBehaviour _flyBehaviour;
    [SerializeField] private WalkingBehaviour _walkingBehaviour;
    [SerializeField] private bool isLastLevel;
    
    [SerializeField] private UnityEvent OnFlyToggled;
    
    public void ToggleFly()
    {
        if (player.isFlying)
        {
            player.TouchesGround();
            player.isFlying = false;
        }
        else
        {
            
            player.Fly();
            player.isFlying = true;
        }
        
        OnFlyToggled?.Invoke();
    }

    public void PassLevel()
    {
        if (isLastLevel)
        {
            levelManager.HandleWinGame();
        }
        else
        {
            levelManager.HandleWin();
        }
    }
}
