using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void HandleWalk()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    public void HandleRun()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
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
