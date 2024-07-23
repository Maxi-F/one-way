using PlayerScripts.FSM;

namespace PlayerScripts.Behaviours
{
    public interface IBehaviour : IFsmBehaviour<MovementBehaviour>
    {
        /// <summary>
        /// Executes the enter to state logic.
        /// </summary>
        /// <param name="previousBehaviour">previous behaviour state.</param>
        public void Enter(IBehaviour previousBehaviour);
        
        /// <summary>
        /// Executes the exit to state logic.
        /// </summary>
        /// <param name="nextBehaviour">next behaviour state.</param>
        public void Exit(IBehaviour nextBehaviour);
    }
}
