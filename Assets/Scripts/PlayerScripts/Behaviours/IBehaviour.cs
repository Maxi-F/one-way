using UnityEngine;

namespace PlayerScripts
{
    public interface IBehaviour
    {
        string GetName();
        void Enter();
        void OnBehaviourUpdate();
        void OnBehaviourFixedUpdate();
        void Exit();
    }
}
