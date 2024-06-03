using System;
using PlayerScripts;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Player player;
    
    [Header("Follow Properties")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -5);
    [SerializeField] private float followSpeed = 5;
    
    [Header("Rotation properties")]
    [SerializeField] private Vector2 xMinMaxRotation = new Vector2(0f, 50f);
    
    private Vector2 _desiredRotation;
    
    private void FixedUpdate()
    {
        ModifyPosition();
    }

    private void LateUpdate()
    {
        ModifyRotation();
    }

    private void ModifyRotation()
    {
        transform.RotateAround(target.position, Vector3.up, _desiredRotation.x * player.Sensibility * Time.deltaTime);
        
        Quaternion previousRotationInX = transform.rotation;
        transform.RotateAround(target.position, transform.right, _desiredRotation.y * player.Sensibility * Time.deltaTime);

        if (transform.rotation.eulerAngles.x < xMinMaxRotation.x || transform.rotation.eulerAngles.x > xMinMaxRotation.y)
        {
            transform.rotation = previousRotationInX;
        }
    }

    private void ModifyPosition()
    {
        var rotatedOffset = transform.rotation * offset;
        var offsetEmulatingTransformPoint = target.position + rotatedOffset;

        transform.position = Vector3.Slerp(transform.position, offsetEmulatingTransformPoint, Time.fixedDeltaTime * followSpeed);
    }

    public void RotateCamera(Vector2 lookInput)
    {
        _desiredRotation = lookInput;
    }
}
