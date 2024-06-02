using System;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Header("Follow Properties")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -5);
    [SerializeField] private float followSpeed = 5;
    
    [Header("Rotation properties")]
    [SerializeField] private float rotationSpeed = 5;

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
        transform.RotateAround(target.position, Vector3.up, _desiredRotation.x * rotationSpeed * Time.deltaTime);
        transform.RotateAround(target.position, Vector3.right, _desiredRotation.y * rotationSpeed * Time.deltaTime);
        
        // TODO resolve bug with camera repositioning
        transform.rotation = Quaternion.Euler(
            Math.Clamp(transform.rotation.eulerAngles.x, xMinMaxRotation.x, xMinMaxRotation.y),
            transform.rotation.eulerAngles.y,
            0
            );
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
