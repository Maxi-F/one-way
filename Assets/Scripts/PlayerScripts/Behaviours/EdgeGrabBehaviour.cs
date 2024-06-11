using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// TODO fix and attach IEdgeGrabBehaviour
public class EdgeGrabBehaviour : MonoBehaviour
{
    [Header("Edge grabbing settings")] 
    [SerializeField] private float edgeJumpImpulse;

    [SerializeField] private float powerJumpImpulse;
    [SerializeField] private Vector2 edgeOffset = new Vector2(0.1f, 1);
    
    [Header("Player data")]
    [SerializeField] private Transform feetPivot;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnEdgePositionSetted;

    [SerializeField] private UnityEvent OnEdgeJump;
    
    private Vector3 _edgePosition;
    private Player _player;
    private Rigidbody _rigidbody;
    private bool _shouldJump = false;
    private EdgeJumpBehaviour _edgeJumpBehaviour;
    private Vector3 _edgeNormal;

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
        // TODO climb up
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
        if (!_shouldJump)
        {
            transform.position = _edgePosition;
            transform.forward = -_edgeNormal;
        }
    }

    public void OnBehaviourFixedUpdate()
    {
        /* TODO fix
        if (_shouldJump)
        {
            _rigidbody.AddForce(Vector3.up * (_player.UseAccumulativeForceOnJump ? powerJumpImpulse : edgeJumpImpulse), ForceMode.Impulse);
            _player.SetBehaviour(_edgeJumpBehaviour);
            _shouldJump = false;
            OnEdgeJump.Invoke();
        }
        else
        {
            _rigidbody.useGravity = false;
        }
        */
    }
    
    public void SetEdgePosition(Transform aTransform, RaycastHit edgeForwardHit, RaycastHit edgeDownHit)
    {
        _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Impulse);

        Vector3 hangPosition = new Vector3(edgeForwardHit.point.x, edgeDownHit.point.y, edgeForwardHit.point.z);
        Vector3 offset = transform.forward * -edgeOffset.x + transform.up * -edgeOffset.y;
        hangPosition += offset;
        
        _edgePosition = hangPosition;
        _edgeNormal = edgeForwardHit.normal;
        
        OnEdgePositionSetted.Invoke();
    }
}
