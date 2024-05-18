using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

public class EdgeGrabBehaviour : MonoBehaviour, IEdgeGrabBehaviour
{
    [Header("Edge grabbing settings")] 
    [SerializeField] private float edgeJumpImpulse;
    
    [Header("Player data")]
    [SerializeField] private Transform feetPivot;

    [Header("Player Behaviours")]
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] WalkingBehaviour walkingBehaviour;
    
    private Vector3 _edgePosition;
    private Player _player;
    private Rigidbody _rigidbody;
    private bool _shouldJump = false;

    public void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _player ??= GetComponent<Player>();
        walkingBehaviour ??= GetComponent<WalkingBehaviour>();
    }
    
    public string GetName()
    {
        return "Edge Grab Behaviour";
    }

    public void Move(Vector3 direction)
    {
        if (Vector3.Dot(direction, transform.forward) < 0)
        {
            _rigidbody.useGravity = true;
            _player.SetBehaviour(walkingBehaviour);
        }
    }

    public void Jump()
    {
        Debug.Log("Jumped!");
        _rigidbody.useGravity = true;
        _shouldJump = true;
    }

    public void LookChange()
    {
    }

    public void TouchesGround()
    {
    }

    public void OnBehaviourUpdate()
    {
        if(!_shouldJump) 
            transform.position = _edgePosition;
    }

    public void OnBehaviourFixedUpdate()
    {
        if (_shouldJump)
        {
            Debug.Log("Adding impulse");
            _rigidbody.AddForce(Vector3.up * edgeJumpImpulse, ForceMode.Impulse);
            _player.SetBehaviour(walkingBehaviour);
            _shouldJump = false;
        }
        else
        {
            _rigidbody.useGravity = false;
        }
    }
    
    public void SetEdgePosition(Transform transform)
    {
        _edgePosition = transform.position;
    }
}
