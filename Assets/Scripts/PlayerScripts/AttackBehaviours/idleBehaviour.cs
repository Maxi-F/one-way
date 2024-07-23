using UnityEngine;

namespace PlayerScripts.AttackBehaviours
{
    public class IdleBehaviour : MonoBehaviour, IAttackBehaviour
    {
        public AttackBehaviour GetName()
        {
            return AttackBehaviour.Idle;
        }

        public void OnBehaviourUpdate()
        {
        }

        public void OnBehaviourFixedUpdate()
        {
        }

        public AttackBehaviour[] GetNextBehaviours()
        {
            return new[] { AttackBehaviour.Attack };
        }

        public void Enter(IAttackBehaviour previousBehaviour)
        {
        }

        public void Exit(IAttackBehaviour nextBehaviour)
        {
        }
    }
}
