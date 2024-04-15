using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private IBehaviour behaviour;

    private bool _shouldBrake;
    private Vector3 _desiredDirection;

    public bool shouldBrake { get { return _shouldBrake; } set { _shouldBrake = value; } }
    public Vector3 desiredDirection { get { return _desiredDirection; } set { _desiredDirection = value; } }

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
        desiredDirection = direction;

        if (direction.magnitude < 0.0001f)
        {
            _shouldBrake = true;
        }

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
