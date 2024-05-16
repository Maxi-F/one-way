using UnityEngine;

namespace PlayerScripts
{
    public interface IBehaviour
    {
        string GetName();
        void Move(Vector3 direction);
        void Jump();
        void LookChange();
        void TouchesGround();
        void OnBehaviourUpdate();
        void OnBehaviourFixedUpdate();
    }
}
