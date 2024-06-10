using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class WalkingBehaviour : MonoBehaviour, IBehaviour
    {
        [Header("Walk Controller")]
        [SerializeField] private MoveController moveController;

        [SerializeField] private UnityEvent OnJump;

        public void Enter()
        {
            moveController.TouchesGround();
        }

        public void OnBehaviourUpdate()
        {
            moveController.OnUpdate();
        }

        public string GetName()
        {
            return "Walking Behaviour";
        }
        
        public void OnBehaviourFixedUpdate()
        {
            moveController.OnFixedUpdate();
        }

        public void Exit()
        {

        }
    }
}
