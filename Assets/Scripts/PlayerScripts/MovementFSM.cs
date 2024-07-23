using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayerScripts.Behaviours;
using PlayerScripts.FSM;

namespace PlayerScripts
{
    public enum MovementBehaviour
    {
        Move = 0,
        Jump,
        EdgeGrab,
        Fly
    }

    public class MovementFsm : Fsm<IBehaviour, MovementBehaviour>
    {
        private readonly List<IBehaviour> _behaviours = new List<IBehaviour>();
        
        
        public MovementFsm(IBehaviour[] behavioursToUse, IBehaviour initBehaviour) {
            _behaviours.AddRange(behavioursToUse);
            CurrentBehaviour = initBehaviour;
        }

        /// <summary>
        /// Change behaviours applying exit and enter.
        /// </summary>
        protected override void ChangeCurrentStateTo(MovementBehaviour nextBehaviourName)
        {
            IBehaviour nextBehaviour = _behaviours.Find(aBehaviour => aBehaviour.GetName() == nextBehaviourName);
            
            CurrentBehaviour.Exit(nextBehaviour);
            nextBehaviour.Enter(CurrentBehaviour);

            CurrentBehaviour = nextBehaviour;
        }
    }
}
