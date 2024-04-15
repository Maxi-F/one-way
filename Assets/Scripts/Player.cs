using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private IBehaviour behaviour;

    private bool _shouldBrake;
    private Vector3 _desiredDirection;
    private Vector3 _obtainedDirection;

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
        _desiredDirection = direction;
        _obtainedDirection = direction;

        _shouldBrake = direction.magnitude < 0.0001f;

        Transform localTransform = transform;
        var camera = Camera.main;
        if (camera != null)
            localTransform = camera.transform;
        _desiredDirection = localTransform.TransformDirection(_desiredDirection);
        _desiredDirection.y = 0;

        Debug.Log($"{name}: desired direction {_desiredDirection.normalized}");
    }

    public void LookChange()
    {
        _desiredDirection = transform.TransformDirection(_obtainedDirection);
        _desiredDirection.y = 0;
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
