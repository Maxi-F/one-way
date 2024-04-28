using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float speed = 12;
    [SerializeField] private float acceleration = 15;
    [SerializeField] private float brakeMultiplier = .60f;
    [SerializeField] private float changeDirectionMultiplier = 1.25f;
    [SerializeField] private float onTouchingGroundImpulse = 10f;
    [SerializeField] private float maxAngleToChangeDirection = 45f;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Player player;
    [SerializeField] private JumpBehaviour jumpBehaviour;

    private Vector3 _obtainedDirection;
    private Vector3 _desiredDirection;
    private bool _shouldBrake;
    private bool _isTouchingGround = false;

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

    public void LookChange()
    {
        _desiredDirection = transform.TransformDirection(_obtainedDirection);
        _desiredDirection.y = 0;
    }

    public void OnBehaviourUpdate()
    {
    }

    public void Move(Vector3 direction)
    {
        if (direction.magnitude < 0.0001f && jumpBehaviour.IsOnFloor())
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

    public void TouchesGround()
    {
        _isTouchingGround = true;
    }

    public void Jump()
    {
        player.SetBehaviour(jumpBehaviour);
        player.Jump();
    }

    public string getName()
    {
        return "Walking Behaviour";
    }

    public void OnBehaviourFixedUpdate()
    {
        Vector3 currentHorizontalVelocity = rigidBody.velocity;
        currentHorizontalVelocity.y = 0;
        float currentSpeed = currentHorizontalVelocity.magnitude;

        float angleBetweenVelocityAndDirection = Vector3.Angle(currentHorizontalVelocity, _desiredDirection);
        Debug.Log(angleBetweenVelocityAndDirection);

        if (currentSpeed < speed)
            rigidBody.AddForce(
                _desiredDirection.normalized * 
                    acceleration * 
                    (angleBetweenVelocityAndDirection > maxAngleToChangeDirection ? changeDirectionMultiplier : 1.0f),
                ForceMode.Force
            );
        if (_shouldBrake)
        {
            rigidBody.AddForce(-currentHorizontalVelocity * brakeMultiplier, ForceMode.Impulse);
            _shouldBrake = false;
        }
        if(_isTouchingGround)
        {
            rigidBody.AddForce(_desiredDirection.normalized * onTouchingGroundImpulse, ForceMode.Impulse);
            _isTouchingGround = false;
        }
    }
}
