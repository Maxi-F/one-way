using System.Collections;
using PlayerScripts.Behaviours;
using UnityEngine;

namespace PlayerScripts.FSM
{
    public abstract class Fsm<TBehaviour, TBehaviourEnum> where TBehaviour : IFsmBehaviour<TBehaviourEnum>
    {
        protected TBehaviour CurrentBehaviour;

        /// <summary>
        /// Internally changes behaviours. It's implemented by the concrete FSM.
        /// </summary>
        /// <param name="nextBehaviourName">Next behaviour to set the FSM to</param>
        protected abstract void ChangeCurrentStateTo(TBehaviourEnum nextBehaviourName);
        
        /// <summary>
        /// Changes the current state of the FSM to a new behaviour state
        /// </summary>
        /// <param name="nextBehaviourName">Next behaviour to set the FSM to</param>
        public void ChangeStateTo(TBehaviourEnum nextBehaviourName)
        {
            if(!((IList)CurrentBehaviour.GetNextBehaviours()).Contains(nextBehaviourName))
            {
                Debug.LogError($"Tried to make transition from {CurrentBehaviour.GetName()} to {nextBehaviourName} But does not exist");
                return;
            }

            Debug.Log($"Transitioning from {CurrentBehaviour.GetName()} to {nextBehaviourName}");
            
            ChangeCurrentStateTo(nextBehaviourName);
        }

        public void OnFixedUpdate()
        {
            CurrentBehaviour.OnBehaviourFixedUpdate();
        }

        public void OnUpdate()
        {
            CurrentBehaviour.OnBehaviourUpdate();
        }

        /// <summary>
        /// returns if behaviour is current one or not.
        /// </summary>
        internal bool IsCurrentBehaviour(TBehaviourEnum name)
        {
            return CurrentBehaviour.GetName().Equals(name);
        }
    }
}
