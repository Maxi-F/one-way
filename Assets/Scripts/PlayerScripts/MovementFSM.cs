using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayerScripts.Behaviours;

namespace PlayerScripts
{
    public enum MovementBehaviour
    {
        Move = 0,
        Jump,
        EdgeGrab,
        Fly
    }

    public class MovementFSM
    {
        private readonly List<IBehaviour> _behaviours = new List<IBehaviour>();
        private IBehaviour _currentBehaviour;

        public MovementFSM(IBehaviour[] behavioursToUse, IBehaviour initBehaviour) {
            _behaviours.AddRange(behavioursToUse);
            _currentBehaviour = initBehaviour;
        }
        
        /// <summary>
        /// Changes the current state of the FSM to a new behaviour state
        /// </summary>
        /// <param name="nextBehaviourName">Next behaviour to set the FSM to</param>
        public void ChangeStateTo(MovementBehaviour nextBehaviourName)
        {
            if(!_currentBehaviour.GetNextBehaviours().Contains(nextBehaviourName))
            {
                Debug.LogError($"Tried to make transition from {_currentBehaviour.GetName()} to {nextBehaviourName} But does not exist");
                return;
            }

            Debug.Log($"Transitioning from {_currentBehaviour.GetName()} to {nextBehaviourName}");
            IBehaviour nextBehaviour = _behaviours.Find(aBehaviour => aBehaviour.GetName() == nextBehaviourName);
            
            _currentBehaviour.Exit(nextBehaviour);
            nextBehaviour.Enter(_currentBehaviour);

            _currentBehaviour = nextBehaviour;
        }

        public void OnFixedUpdate()
        {
            _currentBehaviour.OnBehaviourFixedUpdate();
        }

        public void OnUpdate()
        {
            _currentBehaviour.OnBehaviourUpdate();
        }

        /// <summary>
        /// returns if behaviour is current one or not.
        /// </summary>
        internal bool IsCurrentBehaviour(MovementBehaviour name)
        {
            return _currentBehaviour.GetName() == name;
        }
    }
}
