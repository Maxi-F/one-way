using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

// TODO fix and attach ibehaviour
public class FlyBehaviour : MonoBehaviour, IBehaviour
{
    private FlyController _flyController;

    public void Start()
    {
        _flyController ??= GetComponent<FlyController>();
    }
    
    public MovementBehaviour GetName()
    {
        return MovementBehaviour.Fly;
    }

    public void Enter(IBehaviour previousBehaviour)
    {
        _flyController.StartFly();    
    }

    public void OnBehaviourUpdate()
    {
    }

    public void OnBehaviourFixedUpdate()
    {
        _flyController.Fly();
    }

    public void Exit(IBehaviour nextBehaviour)
    {
        _flyController.EndFly();
    }

    public MovementBehaviour[] GetNextBehaviours()
    {
        return new[] { MovementBehaviour.Move };
    }
}
