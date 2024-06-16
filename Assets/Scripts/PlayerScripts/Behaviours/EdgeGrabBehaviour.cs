using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class EdgeGrabBehaviour : MonoBehaviour, IBehaviour
{
    private EdgeJumpController _edgeJumpController;
    private EdgeGrabController _edgeGrabController;
    
    public void Start()
    {
        _edgeGrabController ??= GetComponent<EdgeGrabController>();
        _edgeJumpController ??= GetComponent<EdgeJumpController>();
    }
    
    public MovementBehaviour GetName()
    {
        return MovementBehaviour.EdgeGrab;
    }

    public void Enter(IBehaviour previousBehaviour)
    {
        _edgeGrabController.SetEdgePosition(transform);
    }

    public void OnBehaviourUpdate()
    {
        _edgeGrabController.StayInPlace();
    }

    public void OnBehaviourFixedUpdate()
    {
        _edgeGrabController.EdgeGrab();
    }

    public void Exit(IBehaviour nextBehaviour)
    {
    }

    public MovementBehaviour[] GetNextBehaviours()
    {
        MovementBehaviour[] nextBehaviours = { MovementBehaviour.EdgeJump };

        return nextBehaviours;
    }
}
