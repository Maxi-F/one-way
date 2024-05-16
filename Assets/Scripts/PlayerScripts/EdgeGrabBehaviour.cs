using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class EdgeGrabBehaviour : MonoBehaviour, IEdgeGrabBehaviour
{
    [SerializeField] Transform feetPivot;
    
    private Collider _collider;
    private Player _player;
    private WalkingBehaviour _walkingBehaviour;
    private Rigidbody _rigidbody;

    public void Start()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
        _player ??= GetComponent<Player>();
        _walkingBehaviour ??= GetComponent<WalkingBehaviour>();
    }
    
    public string GetName()
    {
        return "Edge Grab Behaviour";
    }

    public void Move(Vector3 direction)
    {
    }

    public void Jump()
    {
    }

    public void LookChange()
    {
    }

    public void TouchesGround()
    {
    }

    public void OnBehaviourUpdate()
    {
        _rigidbody.useGravity = false;

        if (feetPivot.position.y - _collider.transform.position.y < 0.5f)
        {
            var vector3 = transform.position;
            vector3.y += 10.0f * Time.deltaTime;
            transform.position = vector3;
        } else if (!_player.IsRaycastOnFloor())
        {
            transform.position =
                Vector3.Slerp(transform.position, _collider.transform.position, 10.0f * Time.deltaTime);
        }
        else
        {
            _rigidbody.useGravity = true;
            _player.SetBehaviour(_walkingBehaviour);
            _player.TouchesGround();
        }
    }

    public void OnBehaviourFixedUpdate()
    {
        
    }
    
    public void SetCollider(Collider collider)
    {
        _collider = collider;
    }
}
