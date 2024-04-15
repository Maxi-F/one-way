using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 12;
    [SerializeField] private float acceleration = 15;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float brakeMultiplier = .75f;
    [SerializeField] private JumpBehaviour jumpBehaviour;

    private Vector3 _obtainedDirection;
    private Vector3 _desiredDirection;
    private bool _shouldBrake;
    private bool _isJumping = false;

    public bool isJumping { get { return _isJumping; } set { _isJumping = value; } }

    public float CurrentSpeed
    {
        get
        {
            return _desiredDirection.magnitude * speed;
        }
    }

    private void Reset()
    {
        rigidBody ??= GetComponent<Rigidbody>();
        jumpBehaviour ??= GetComponent<JumpBehaviour>();
    }

    public void Update()
    {
        if(_isJumping && jumpBehaviour.IsOnFloor())
        {
            Debug.Log($"{name}: Is on floor!");
            _isJumping = false;
        }
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
        if (direction.magnitude < 0.0001f && !_isJumping)
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
        var currentHorizontalVelocity = rigidBody.velocity;
        currentHorizontalVelocity.y = 0;
        var currentSpeed = currentHorizontalVelocity.magnitude;
        if (currentSpeed < speed && !_isJumping)
            rigidBody.AddForce(_desiredDirection.normalized * acceleration, ForceMode.Force);
        if (_shouldBrake && !_isJumping)
        {
            rigidBody.AddForce(-currentHorizontalVelocity * brakeMultiplier, ForceMode.Impulse);
            _shouldBrake = false;
        }
    }
}
