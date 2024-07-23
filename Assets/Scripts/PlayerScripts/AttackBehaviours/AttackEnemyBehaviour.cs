using System;
using PlayerScripts.Controllers;
using UnityEngine;

namespace PlayerScripts.AttackBehaviours
{
    public class AttackEnemyBehaviour : MonoBehaviour, IAttackBehaviour
    {
        private ParryController _parryController;

        private void Start()
        {
            _parryController ??= GetComponent<ParryController>();
        }

        public AttackBehaviour GetName()
        {
            return AttackBehaviour.Attack;
        }

        public void OnBehaviourUpdate()
        {
            _parryController.OnUpdate();
        }

        public void OnBehaviourFixedUpdate()
        {
        }

        public AttackBehaviour[] GetNextBehaviours()
        {
            return new[] { AttackBehaviour.Idle };
        }

        public void Enter(IAttackBehaviour previousBehaviour)
        {
            _parryController.Attack();
        }

        public void Exit(IAttackBehaviour nextBehaviour)
        {
            _parryController.SetBuffer();
        }
    }
}
