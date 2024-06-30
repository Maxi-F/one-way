using UnityEngine;

namespace Animations
{
    public class DoubleJumpExit : StateMachineBehaviour
    {
        private string isDoubleJumpingEvent = "isDoubleJumping";
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isDoubleJumpingEvent, false);
        }
    }
}
