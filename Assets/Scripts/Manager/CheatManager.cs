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
        if (player.IsFlying)
        {
            player.TouchesGround();
            player.IsFlying = false;
        }
        else
        {
            
            player.Fly();
            player.IsFlying = true;
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
