namespace PlayerScripts.Behaviours
{
    public interface IBehaviour
    {
        /// <summary>
        /// Gets the name of the behaviour.
        /// </summary>
        /// <returns>a movement behaviour enum of the name.</returns>
        MovementBehaviour GetName();
        
        /// <summary>
        /// Executes the enter to state logic.
        /// </summary>
        /// <param name="previousBehaviour">previous behaviour state.</param>
        void Enter(IBehaviour previousBehaviour);
        
        /// <summary>
        /// Executes behaviour update logic.
        /// </summary>
        void OnBehaviourUpdate();
        
        /// <summary>
        /// Executes behaviour fixed update logic.
        /// </summary>
        void OnBehaviourFixedUpdate();
        
        /// <summary>
        /// Executes the exit to state logic.
        /// </summary>
        /// <param name="previousBehaviour">next behaviour state.</param>
        void Exit(IBehaviour nextBehaviour);
        
        /// <summary>
        /// Returns the possible next behaviour states.
        /// </summary>
        MovementBehaviour[] GetNextBehaviours();
    }
}
