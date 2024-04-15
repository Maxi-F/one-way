using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] Transform feetPivot;
    [SerializeField] private float force;
    [SerializeField] private LayerMask floor;
    [SerializeField] private float groundedDistance = 0.1f;
    [SerializeField] private float jumpingMiliseconds = 100f;

    private Ray _groundRay;
    private Rigidbody _rigidBody;
    private bool _shouldJump = false;
    private float _timeJumped = 0f;

    private void Start()
    {
        _rigidBody ??= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_shouldJump && CanJump())
        {
            _rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
            _shouldJump = false;
        }
    }

    public bool TryJump()
    {
        if(IsOnFloor())
        {
            Debug.Log($"{name}: Jump!");
            _shouldJump = true;
            _timeJumped = Time.time * 1000f;
            return true;
        }
        return false;
    }

    private bool JumpingBreakTime()
    {
        return _timeJumped + jumpingMiliseconds < (Time.time * 1000f);
    }

    private bool IsRaycastOnFloor()
    {
        return Physics.Raycast(feetPivot.position, Vector3.down, out var hit, groundedDistance, floor);
    }

    public bool IsOnFloor()
    {
        return IsRaycastOnFloor() && JumpingBreakTime();
    }

    public bool CanJump()
    {
        return feetPivot && IsRaycastOnFloor();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(feetPivot.position, Vector3.down * groundedDistance);
    }
}
