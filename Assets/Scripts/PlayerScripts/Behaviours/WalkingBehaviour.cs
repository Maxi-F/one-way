using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class WalkingBehaviour : MonoBehaviour, IBehaviour
    {
        [Header("Walk Controller")]
        [SerializeField] private MoveController moveController;
        [SerializeField] private JumpController jumpController;
        [SerializeField] private PlayerController playerController;

        [SerializeField] private UnityEvent OnJump;

        public void Enter(IBehaviour previousBehaviour)
        {
            if(previousBehaviour.GetName() == MovementBehaviour.Jump)
                moveController.TouchesGround();
        }

        public void OnBehaviourUpdate()
        {
            playerController.ResetCoyoteTime();
            moveController.CheckVelocity();
            moveController.Events();
        }

        public MovementBehaviour GetName()
        {
            return MovementBehaviour.Move;
        }
        
        public void OnBehaviourFixedUpdate()
        {
            moveController.MoveInGround();
        }

        public void Exit(IBehaviour nextBehaviour)
        {
            if(nextBehaviour.GetName() == MovementBehaviour.Jump)
            {
                if (playerController.IsOnFloor() || playerController.IsOnCoyoteTimeFloor()) {
                    jumpController.SetShouldJumpValues();
                } else
                {
                    jumpController.SetIsJumping();
                }
                
                OnJump.Invoke();
            }
        }

        public MovementBehaviour[] GetNextBehaviours()
        {
            MovementBehaviour[] nextBehaviours = { MovementBehaviour.Jump, MovementBehaviour.Fly };

            return nextBehaviours;
        }
    }
}
