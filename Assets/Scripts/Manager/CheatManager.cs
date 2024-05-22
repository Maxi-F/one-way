using System.Collections;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Player player;
    [SerializeField] private FlyBehaviour _flyBehaviour;
    [SerializeField] private WalkingBehaviour _walkingBehaviour;
    [SerializeField] private bool isLastLevel;
    
    public void ToggleFly()
    {
        if (player.isFlying)
        {
            player.SetBehaviour(_walkingBehaviour);
            player.SetGravity(true);
            player.isFlying = false;
        }
        else
        {
            player.SetBehaviour(_flyBehaviour);
            player.SetGravity(false);
            player.isFlying = true;
        }
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
