using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class EdgeJumpBehaviour : MonoBehaviour, IBehaviour
{
    private WalkingBehaviour _walkingBehaviour;
    private JumpBehaviour _jumpBehaviour;
    private Player _player;
    public void Start()
    {
        _player = GetComponent<Player>();
        _walkingBehaviour ??= GetComponent<WalkingBehaviour>();
        _jumpBehaviour ??= GetComponent<JumpBehaviour>();
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
        }
    }

    public void OnBehaviourFixedUpdate()
    {
        if (!_jumpBehaviour.IsOnFloor())
        {
            _walkingBehaviour.OnBehaviourFixedUpdate();
        }
    }
}
