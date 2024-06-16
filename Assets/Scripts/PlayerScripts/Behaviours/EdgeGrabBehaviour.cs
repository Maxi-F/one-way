using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class EdgeGrabBehaviour : MonoBehaviour, IBehaviour
{
    private JumpController _jumpController;
    private EdgeGrabController _edgeGrabController;
    
    public void Start()
    {
        _edgeGrabController ??= GetComponent<EdgeGrabController>();
        _jumpController ??= GetComponent<JumpController>();
    }
    
    public MovementBehaviour GetName()
    {
        return MovementBehaviour.EdgeGrab;
    }

    public void Enter(IBehaviour previousBehaviour)
    {
        _edgeGrabController.SetEdgePosition();
        _edgeGrabController.IsEdgeGrabbing = true;
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
        _edgeGrabController.SetEdgeGrabTimer();
        _edgeGrabController.IsEdgeGrabbing = false;
        _jumpController.SetShouldJumpValues();
    }

    public MovementBehaviour[] GetNextBehaviours()
    {
        MovementBehaviour[] nextBehaviours = { MovementBehaviour.Jump };

        return nextBehaviours;
    }
}
