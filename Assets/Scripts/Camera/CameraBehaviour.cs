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
    [SerializeField] private Vector2 topMinMaxAngles = new Vector2(80.0f, 100.0f);
    [SerializeField] private Vector2 bottomMinMaxAngles = new Vector2(260.0f, 280.0f);
    
    private Vector2 _desiredRotation;
    
    private void FixedUpdate()
    {
        ModifyRotation();
        ModifyPosition();
    }

    private void ModifyRotation()
    {
        Quaternion previousRotationInX = transform.rotation;
        Vector3 previousPositionInX = transform.position;
        transform.RotateAround(target.position, transform.right, _desiredRotation.y * player.Sensibility * Time.fixedDeltaTime);

        float angleInX = transform.rotation.eulerAngles.x;
        
        if ((angleInX > topMinMaxAngles.x && angleInX < topMinMaxAngles.y) ||
            (angleInX > bottomMinMaxAngles.x && angleInX < bottomMinMaxAngles.y))
        {
            transform.rotation = previousRotationInX;
            transform.position = previousPositionInX;
        }
        
        transform.RotateAround(target.position, Vector3.up, _desiredRotation.x * player.Sensibility * Time.fixedDeltaTime);
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
