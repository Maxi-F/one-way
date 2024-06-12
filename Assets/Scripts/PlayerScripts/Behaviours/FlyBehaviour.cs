using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

// TODO fix and attach ibehaviour
public class FlyBehaviour : MonoBehaviour
{
    [SerializeField] private float velocity;
    private Vector3 _direction = new Vector3(0, 0, 0);
    private bool _isGoingUp = false;
    private Player _player;
    
    public bool GoDown { get; set; }
    void Start()
    {
        _player ??= GetComponent<Player>();
    }
    
    public string GetName()
    {
        return "Fly Behaviour";
    }

    public void Move(Vector3 direction)
    {
        Vector3 _obtainedDirection = direction;
        Transform localTransform = transform;
        var mainCamera = Camera.main;
        if (mainCamera != null)
            localTransform = mainCamera.transform;
        _direction = localTransform.TransformDirection(_obtainedDirection);
        _direction.y = 0;
    }

    public void Jump()
    {
        _isGoingUp = !_isGoingUp;
    }

    public void LookChange()
    {
    }

    public void TouchesGround()
    {
    }

    public void OnBehaviourUpdate()
    {
        Vector3 upVector = _isGoingUp ? Vector3.up : Vector3.zero;
        Vector3 downVector = GoDown ? Vector3.down : Vector3.zero;
        
        transform.position += (_direction + upVector + downVector) * velocity * Time.deltaTime;
    }

    public void OnBehaviourFixedUpdate()
    {
    }
}
