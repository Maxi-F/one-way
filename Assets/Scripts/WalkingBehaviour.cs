using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float speed = 12;
    [SerializeField] private float acceleration = 15;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float brakeMultiplier = .75f;
    [SerializeField] private Player player;
    [SerializeField] private JumpBehaviour jumpBehaviour;

    private void Reset()
    {
        rigidBody ??= GetComponent<Rigidbody>();
        jumpBehaviour ??= GetComponent<JumpBehaviour>();
    }

    public void OnBehaviourUpdate()
    {
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
        var currentHorizontalVelocity = rigidBody.velocity;
        currentHorizontalVelocity.y = 0;
        var currentSpeed = currentHorizontalVelocity.magnitude;
        if (currentSpeed < speed)
        {
            rigidBody.AddForce(player.desiredDirection.normalized * acceleration, ForceMode.Force);
        }
        if (player.shouldBrake)
        {
            rigidBody.AddForce(-currentHorizontalVelocity * brakeMultiplier, ForceMode.Impulse);
            player.shouldBrake = false;
        }
    }
}
