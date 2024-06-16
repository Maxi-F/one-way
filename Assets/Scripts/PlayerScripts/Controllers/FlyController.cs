using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [SerializeField] private float velocity;
    
    private MoveController _moveController; 
        
    private bool _isGoingUp = false;
    private Rigidbody _rigidbody;
    
    public bool GoDown { get; set; }

    public void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _moveController ??= GetComponent<MoveController>();
    }
    
    public void StartFly()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
    }

    public void EndFly()
    {
        _rigidbody.useGravity = true;
    }

    public void GoUp()
    {
        _isGoingUp = !_isGoingUp;
    }

    public void Fly()
    {
        Vector3 upVector = _isGoingUp ? Vector3.up : Vector3.zero;
        Vector3 downVector = GoDown ? Vector3.down : Vector3.zero;
        
        transform.position += (_moveController.Direction + upVector + downVector) * (velocity * Time.deltaTime);
    }
}
