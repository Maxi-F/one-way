using UnityEngine;

namespace PlayerScripts
{
    public interface IEdgeGrabBehaviour : IBehaviour
    {
        void SetEdgePosition(Transform transform);
    }
}