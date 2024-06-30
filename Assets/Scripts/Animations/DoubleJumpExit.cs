using UnityEngine;

namespace Animations
{
    public class DoubleJumpExit : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("isDoubleJumping", false);
        }
    }
}
