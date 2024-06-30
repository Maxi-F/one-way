using PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animation properties")]
    [SerializeField] [Range(0.0f, 1.0f)] private float walkingVelocityPercentage = 0.1f;
    [SerializeField] [Range(0.0f, 1.0f)] private float runningVelocityPercentage = 0.1f;

    private Player _player;

    public void Start()
    {
        _player ??= GetComponent<Player>();
    }

    public void HandleWalk()
    {
        Vector3 horizontalVelocity = _player.GetHorizontalVelocity();
        if (horizontalVelocity.magnitude < 0.0001f) return;
        
        if(horizontalVelocity.magnitude < _player.VelocityToRun)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetFloat("walkingSpeed", Mathf.Clamp(_player.GetHorizontalVelocityMagnitude() * walkingVelocityPercentage, 1, 2));
        } else
        {
            animator.SetBool("isRunning", true);
            animator.SetFloat("runningSpeed", Mathf.Clamp(_player.GetHorizontalVelocityMagnitude() * runningVelocityPercentage, 1, 2));
        }

    }

    public void HandleJump()
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void HandleDeath()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isHanging", false);
    } 

    public void HandleFall()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isDoubleJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isFalling", true);
    }

    public void HandleDoubleJump()
    {
        animator.SetBool("isDoubleJumping", true);
    }

    public void HandleInFloor()
    {
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        HandleWalk();
    }

    public void HandleBreak()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void HandleHang()
    {
        animator.SetBool("isHanging", true);
    }

    public void HandleLetGoHang()
    {
        animator.SetBool("isHanging", false);
    }
}
