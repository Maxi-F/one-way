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

        [SerializeField] private UnityEvent OnJump;

        public void Enter(IBehaviour previousBehaviour)
        {
            if(previousBehaviour.GetName() == MovementBehaviour.Jump)
                moveController.TouchesGround();
        }

        public void OnBehaviourUpdate()
        {
            moveController.OnUpdate();
        }

        public MovementBehaviour GetName()
        {
            return MovementBehaviour.Move;
        }
        
        public void OnBehaviourFixedUpdate()
        {
            moveController.OnFixedUpdate();
        }

        public void Exit(IBehaviour nextBehaviour)
        {
            if(nextBehaviour.GetName() == MovementBehaviour.Jump)
            {
                jumpController.Jump();
                OnJump.Invoke();
            }
        }

        public MovementBehaviour[] GetNextBehaviours()
        {
            MovementBehaviour[] nextBehaviours = { MovementBehaviour.Jump };

            return nextBehaviours;
        }
    }
}
