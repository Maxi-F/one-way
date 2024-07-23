using PlayerScripts.FSM;

namespace PlayerScripts.AttackBehaviours
{
    /// <summary>
    /// Interface used for attack behaviours in the Attacks FSM
    /// </summary>
    public interface IAttackBehaviour : IFsmBehaviour<AttackBehaviour>
    {
        /// <summary>
        /// Executes the enter to state logic.
        /// </summary>
        /// <param name="previousBehaviour">previous behaviour state.</param>
        public void Enter(IAttackBehaviour previousBehaviour);
        
        /// <summary>
        /// Executes the exit to state logic.
        /// </summary>
        /// <param name="nextBehaviour">next behaviour state.</param>
        public void Exit(IAttackBehaviour nextBehaviour);
    }
}
