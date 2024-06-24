using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        List<IBehaviour> behaviours = new List<IBehaviour>();
        IBehaviour currentBehaviour;

        public MovementFSM(IBehaviour[] behavioursToUse, IBehaviour initBehaviour) {
            behaviours.AddRange(behavioursToUse);
            currentBehaviour = initBehaviour;
        }
        
        public void changeStateTo(MovementBehaviour nextBehaviourName)
        {
            if(!currentBehaviour.GetNextBehaviours().Contains(nextBehaviourName))
            {
                Debug.LogError($"Tried to make transition from {currentBehaviour.GetName()} to {nextBehaviourName} But does not exist");
                return;
            }

            Debug.Log($"Transitioning from {currentBehaviour.GetName()} to {nextBehaviourName}");
            IBehaviour nextBehaviour = behaviours.Find(aBehaviour => aBehaviour.GetName() == nextBehaviourName);
            
            currentBehaviour.Exit(nextBehaviour);
            nextBehaviour.Enter(currentBehaviour);

            currentBehaviour = nextBehaviour;
        }

        public void OnFixedUpdate()
        {
            currentBehaviour.OnBehaviourFixedUpdate();
        }

        public void OnUpdate()
        {
            currentBehaviour.OnBehaviourUpdate();
        }

        internal bool IsCurrentBehaviour(MovementBehaviour name)
        {
            return currentBehaviour.GetName() == name;
        }
    }
}
