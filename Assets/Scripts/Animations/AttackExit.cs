using UnityEngine;

namespace Animations
{
    public class AttackExit : StateMachineBehaviour
    {
        private string isAttackingEvent = "isAttacking";
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isAttackingEvent, false);
        }
    }
}
