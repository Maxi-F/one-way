using PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animation properties")]
    [SerializeField] private float velocityToRun;

    private Player _player;

    public void Start()
    {
        _player ??= GetComponent<Player>();
    }

    public void HandleWalk()
    {
        Debug.Log($"{_player.velocity}, {velocityToRun}, {_player.velocity < velocityToRun}");
        if(_player.velocity < velocityToRun)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isRunning", true);
        }

    }

    public void HandleJump()
    {
        animator.SetBool("isJumping", true);
    }

    public void HandlePowerJump()
    {
        animator.SetBool("isPowerJumping", true);
    }

    public void HandleFall()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isPowerJumping", false);
        animator.SetBool("isFalling", true);
    }

    public void HandleInFloor()
    {
        animator.SetBool("isFalling", false);
    }

    public void HandleBreak()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
}
