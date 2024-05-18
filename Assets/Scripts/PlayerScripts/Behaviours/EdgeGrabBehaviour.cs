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
    
    private Vector3 _edgePosition;
    private Player _player;
    private Rigidbody _rigidbody;
    private bool _shouldJump = false;
    private EdgeJumpBehaviour _edgeJumpBehaviour;

    public void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _player ??= GetComponent<Player>();
        _edgeJumpBehaviour ??= GetComponent<EdgeJumpBehaviour>();
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
            _player.SetBehaviour(_edgeJumpBehaviour);
        }
    }

    public void Jump()
    {
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
            _rigidbody.AddForce(Vector3.up * edgeJumpImpulse, ForceMode.Impulse);
            _player.SetBehaviour(_edgeJumpBehaviour);
            _shouldJump = false;
        }
        else
        {
            _rigidbody.useGravity = false;
        }
    }
    
    public void SetEdgePosition(Transform aTransform)
    {
        _edgePosition = aTransform.position;
    }
}
