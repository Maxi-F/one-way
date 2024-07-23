using System.Collections.Generic;
using PlayerScripts.AttackBehaviours;
using PlayerScripts.Behaviours;
using PlayerScripts.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public enum AttackBehaviour
    {
        Idle = 0,
        Attack
    }
    
    public class AttackFsm : Fsm<IAttackBehaviour, AttackBehaviour>
    {
        private readonly List<IAttackBehaviour> _behaviours = new List<IAttackBehaviour>();
        
        public AttackFsm(IAttackBehaviour[] behavioursToUse, IAttackBehaviour initBehaviour) {
            _behaviours.AddRange(behavioursToUse);
            CurrentBehaviour = initBehaviour;
        }

        /// <summary>
        /// Change behaviours applying exit and enter.
        /// </summary>
        protected override void ChangeCurrentStateTo(AttackBehaviour nextBehaviourName)
        {
            IAttackBehaviour nextBehaviour = _behaviours.Find(aBehaviour => aBehaviour.GetName() == nextBehaviourName);
            
            CurrentBehaviour.Exit(nextBehaviour);
            nextBehaviour.Enter(CurrentBehaviour);

            CurrentBehaviour = nextBehaviour;
        }
    }
}
