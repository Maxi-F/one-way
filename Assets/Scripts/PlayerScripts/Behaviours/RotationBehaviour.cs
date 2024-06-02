using System;
using UnityEngine;

namespace PlayerScripts
{
    public class RotationBehaviour : MonoBehaviour
    {
        private WalkingBehaviour _walkingBehaviour;

        private void Start()
        {
            _walkingBehaviour ??= GetComponent<WalkingBehaviour>();
        }

        public void LookInDirection()
        {
            
            transform.LookAt(transform.position + _walkingBehaviour.direction);
        }
    }
}
