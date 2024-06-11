using UnityEngine;

namespace PlayerScripts
{
    public interface IBehaviour
    {
        MovementBehaviour GetName();
        void Enter(IBehaviour previousBehaviour);
        void OnBehaviourUpdate();
        void OnBehaviourFixedUpdate();
        void Exit(IBehaviour nextBehaviour);
        MovementBehaviour[] GetNextBehaviours();
    }
}
