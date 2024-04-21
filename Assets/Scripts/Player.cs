using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private IBehaviour _behaviour;
    [SerializeField] private RotationBehaviour _rotationBehaviour;

    public void Awake()
    {
        if (_rotationBehaviour == null)
        {
            Debug.LogError($"{name}: {nameof(_rotationBehaviour)} is null!" +
                           $"\nThis class is dependant on a {nameof(_rotationBehaviour)} component!");
        }
    }
    public void Start()
    {
        _behaviour ??= GetComponent<WalkingBehaviour>();
        _rotationBehaviour ??= GetComponent<RotationBehaviour>();
    }

    public void SetBehaviour(IBehaviour newBehaviour)
    {
        Debug.Log($"{name}: Behaviour change to {newBehaviour.getName()}");
        _behaviour = newBehaviour;
    }

    public void Move(Vector3 direction)
    {
        _behaviour.Move(direction);
    }

    public void LookChange(Vector2 eulers)
    {
        
        _behaviour.LookChange();
        _rotationBehaviour.RotateInAngles(eulers.x);
    }

    public void Jump()
    {
        _behaviour.Jump();
    }

    public void Update()
    {
        _behaviour.OnBehaviourUpdate();
    }

    public void FixedUpdate()
    {
        _behaviour.OnBehaviourFixedUpdate();
    }
}
