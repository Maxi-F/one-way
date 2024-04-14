using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 12;
    [SerializeField] private float acceleration = 15;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float brakeMultiplier = .75f;
    private Vector3 _obtainedDirection;
    private Vector3 _desiredDirection;
    private bool _shouldBrake;

    public float CurrentSpeed
    {
        get
        {
            return _desiredDirection.magnitude * speed;
        }
    }

    private void Reset()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void LookChange()
    {
        _desiredDirection = transform.TransformDirection(_obtainedDirection);
        _desiredDirection.y = 0;
    }

    public void Move(Vector3 direction)
    {
        //We need to convert the direction from global to camera.Local
        //direction is currently Global
        if (direction.magnitude < 0.0001f)
        {
            _shouldBrake = true;
        }
        _obtainedDirection = direction;
        Transform localTransform = transform;
        var camera = Camera.main;
        if (camera != null)
            localTransform = camera.transform;
        _desiredDirection = localTransform.TransformDirection(_obtainedDirection);
        _desiredDirection.y = 0;
    }

    private void FixedUpdate()
    {
        Debug.Log(_desiredDirection.normalized);
        var currentHorizontalVelocity = rigidBody.velocity;
        currentHorizontalVelocity.y = 0;
        var currentSpeed = currentHorizontalVelocity.magnitude;
        if (currentSpeed < speed)
            rigidBody.AddForce(_desiredDirection.normalized * acceleration, ForceMode.Force);
        if (_shouldBrake)
        {
            rigidBody.AddForce(-currentHorizontalVelocity * brakeMultiplier, ForceMode.Impulse);
            _shouldBrake = false;
        }
    }
}
