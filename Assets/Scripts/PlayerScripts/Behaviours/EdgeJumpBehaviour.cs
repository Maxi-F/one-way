using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class EdgeJumpBehaviour : MonoBehaviour, IBehaviour
{
    [Header("Edge jump settings")] 
    [SerializeField] private float timeUntilWalkingIsEnabled = 0.5f;
    
    private WalkingBehaviour _walkingBehaviour;
    private JumpBehaviour _jumpBehaviour;
    private EdgeGrabBehaviour _edgeGrabBehaviour;
    private Player _player;
    private float _timePassed = 0f;
    
    public void Start()
    {
        _player = GetComponent<Player>();
        _walkingBehaviour ??= GetComponent<WalkingBehaviour>();
        _jumpBehaviour ??= GetComponent<JumpBehaviour>();
        _edgeGrabBehaviour ??= GetComponent<EdgeGrabBehaviour>();
    }

    public string GetName()
    {
        return "Edge Jump Behaviour";
    }

    public void Move(Vector3 direction)
    {
        _walkingBehaviour.Move(direction);
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
        if (_jumpBehaviour.IsOnFloor())
        {
            _player.SetBehaviour(_walkingBehaviour);
            _player.TouchesGround();
            _timePassed = 0f;
        } else if (_player.IsOnEdge())
        {
            _player.SetBehaviour(_edgeGrabBehaviour);
            _edgeGrabBehaviour.SetEdgePosition(transform);
        }
    }

    public void OnBehaviourFixedUpdate()
    {
        if (!_jumpBehaviour.IsOnFloor())
        {
            if (_timePassed < timeUntilWalkingIsEnabled)
            {
                _timePassed += Time.fixedDeltaTime;
            }
            else
            {
                _walkingBehaviour.OnBehaviourFixedUpdate();
            }
        }
    }
}
