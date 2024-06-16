using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EdgeGrabController : MonoBehaviour
{
    [SerializeField] private LayerMask floor;
    
    [Header("Edge Grabbing Settings")]
    [SerializeField] private float edgeGrabUpLineStartDistance = 1.5f;
    [SerializeField] private float edgeGrabUpLineEndDistance = 0.5f;
    [SerializeField] private float edgeGrabYdistance = 0.1f;
    [SerializeField] private float powerJumpImpulse;
    [SerializeField] private Vector2 edgeOffset = new Vector2(0.1f, 1);
    [SerializeField] private float edgeJumpImpulse = 5.0f;
    [SerializeField] private float secondsUntilEdgeGrabAvailable = 2.0f;
    
    private Vector3 _edgeLineCastStart;
    private Vector3 _edgeLineCastEnd;
    private RaycastHit _edgeForwardHit;
    private RaycastHit _edgeDownHit;

    private Rigidbody _rigidbody;

    private float _timeUntilEdgeGrabAvailable = 0.0f;
    private Vector3 _edgePosition;
    private bool _shouldJump = false;
    private Vector3 _edgeNormal;
    public bool ForceJump { get; set; }
    public bool IsEdgeGrabbing { get; set; }

    void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
    }
    
    public bool IsOnEdge()
    {
        if (!IsEdgeGrabAvailable()) return false;
        
        _edgeLineCastStart = transform.position + transform.up * edgeGrabUpLineStartDistance + transform.forward;
        _edgeLineCastEnd = transform.position + transform.up * edgeGrabUpLineEndDistance + transform.forward;

        if (_rigidbody.velocity.y < 0 && Physics.Linecast(_edgeLineCastStart, _edgeLineCastEnd, out _edgeDownHit, floor))
        {
            Vector3 forwardCastStart = new Vector3(transform.position.x, _edgeDownHit.point.y - edgeGrabYdistance,
                transform.position.z);
            Vector3 forwardCastEnd = forwardCastStart + transform.forward;

            return Physics.Linecast(forwardCastStart, forwardCastEnd, out _edgeForwardHit, floor);
        };
        return false;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
            
        Gizmos.DrawLine(_edgeLineCastStart, _edgeLineCastEnd);
    }
    
    public void StayInPlace()
    {
        transform.position = _edgePosition;
        transform.forward = -_edgeNormal;
    }
    
    public void EdgeGrab()
    {
        if (_shouldJump)
        {
            _rigidbody.AddForce(Vector3.up * (ForceJump ? powerJumpImpulse : edgeJumpImpulse), ForceMode.Impulse);
        }
        else
        {
            _rigidbody.useGravity = false;
        }
    }
    
    public void SetEdgePosition()
    {
        _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Impulse);

        Vector3 hangPosition = new Vector3(_edgeForwardHit.point.x, _edgeDownHit.point.y, _edgeForwardHit.point.z);
        Vector3 offset = transform.forward * -edgeOffset.x + transform.up * -edgeOffset.y;
        hangPosition += offset;
        
        _edgePosition = hangPosition;
        _edgeNormal = new Vector3(_edgeForwardHit.normal.x, 0, _edgeForwardHit.normal.z);
    }

    private bool IsEdgeGrabAvailable()
    {
        return _timeUntilEdgeGrabAvailable < Time.time;
    } 
    
    public void SetEdgeGrabTimer()
    {
        _timeUntilEdgeGrabAvailable = Time.time + secondsUntilEdgeGrabAvailable;
        _rigidbody.useGravity = true;
    }
}
