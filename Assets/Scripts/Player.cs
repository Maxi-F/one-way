using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private IBehaviour behaviour;

    public void Start()
    {
        behaviour ??= GetComponent<WalkingBehaviour>();
    }

    public void SetBehaviour(IBehaviour newBehaviour)
    {
        Debug.Log($"{name}: Behaviour change to {newBehaviour.getName()}");
        behaviour = newBehaviour;
    }

    public void Move(Vector3 direction)
    {
        behaviour.Move(direction);
    }

    public void LookChange()
    {
        behaviour.LookChange();
    }

    public void Jump()
    {
        behaviour.Jump();
    }

    public void Update()
    {
        behaviour.OnBehaviourUpdate();
    }

    public void FixedUpdate()
    {
        behaviour.OnBehaviourFixedUpdate();
    }
}
