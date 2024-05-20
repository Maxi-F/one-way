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
        Debug.Log("Handle walk");
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
        Debug.Log("Handle jump");
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", true);
    }

    public void HandlePowerJump()
    {
        animator.SetBool("isPowerJumping", true);
    }

    public void HandleFall()
    {
        Debug.Log("Handle fall");
        animator.SetBool("isJumping", false);
        animator.SetBool("isPowerJumping", false);
        animator.SetBool("isFalling", true);
    }

    public void HandleInFloor()
    {
        Debug.Log("Handle in floor");
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isPowerJumping", false);
    }

    public void HandleBreak()
    {
        Debug.Log("Handle break");
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
}
