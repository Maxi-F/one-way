using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [Header("Player config")]
    [SerializeField] private Transform feetPivot;
    
    [Header("Floor layer")]
    [SerializeField] private LayerMask floor;
    
    [Header("Grounded Settings")]
    [SerializeField] private float groundedRaycastDistance = 0.2f;
    [SerializeField] private float groundedRideDistance = 0.1f;
    [SerializeField] private float springForce = 5f;
    [SerializeField] private float springDamper = 2.0f;
    
    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    
    private bool _isOnGround;
    private RaycastHit _hit;

    private void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _playerController ??= GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (_isOnGround && _rigidbody.useGravity)
        {
            Vector3 velocity = _rigidbody.velocity;
            Vector3 downRayDirection = feetPivot.transform.TransformDirection(Vector3.down);

            float rayDirVelocity = Vector3.Dot(downRayDirection, velocity);
            float distance = _hit.distance - groundedRideDistance;

            float springForceToUse = (distance * springForce) - (rayDirVelocity * springDamper); 

            _rigidbody.AddForce(downRayDirection * springForceToUse, ForceMode.Force);
        }
    }

    private void Update()
    {
        _isOnGround = Physics.Raycast(
            feetPivot.position, 
            Vector3.down, 
            out _hit, 
            groundedRaycastDistance, 
            floor
            ) && _playerController.JumpingBreakTime();
    }

    public bool IsOnGround()
    {
        return _isOnGround;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(feetPivot.position, Vector3.down * groundedRaycastDistance);
    }
}
