using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, -5);
    public float followSpeed = 5;
    public float rotationSpeed = 5;

    private float _rotationInY;

    public void OnRotateYAngle(float delta)
    {
        _rotationInY = delta;
    }
    
    private void FixedUpdate()
    {
        ModifyRotation();
        
        ModifyPosition();
    }

    private void ModifyRotation()
    {
        var desiredRotation = target.rotation
                              * Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
        
        transform.RotateAround(target.position, target.right, _rotationInY * rotationSpeed * Time.deltaTime);
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
    private void ModifyPosition()
    {
        var rotatedOffset = target.rotation * Quaternion.Euler(transform.eulerAngles.x, 0, 0) * offset;
        var offsetEmulatingTransformPoint = target.position + rotatedOffset;

        transform.position = Vector3.Slerp(transform.position, offsetEmulatingTransformPoint, Time.deltaTime * followSpeed);
    }
}
