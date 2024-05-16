using UnityEngine;

namespace PlayerScripts
{
    public interface IEdgeGrabBehaviour : IBehaviour
    {
        void SetCollider(Collider collider);
    }
}
