using UnityEngine;

namespace PlayerScripts.FSM
{
    public interface IFsmBehaviour<TBehaviourEnum>
    {
        /// <summary>
        /// Gets the name of the behaviour.
        /// </summary>
        /// <returns>behaviour enum of the name.</returns>
        TBehaviourEnum GetName();
        
        /// <summary>
        /// Executes behaviour update logic.
        /// </summary>
        void OnBehaviourUpdate();
        
        /// <summary>
        /// Executes behaviour fixed update logic.
        /// </summary>
        void OnBehaviourFixedUpdate();
        
        /// <summary>
        /// Returns the possible next behaviour states.
        /// </summary>
        TBehaviourEnum[] GetNextBehaviours();
    }
}
