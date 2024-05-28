using System;
using UnityEngine;

namespace PlayerScripts
{
    public class RotationBehaviour : MonoBehaviour
    {
        private float _desiredRotation = 0;
        private float _rotationMultiplier = 100f;
        private Player _player;

        private void Start()
        {
            _player ??= GetComponent<Player>();
        }

        public void RotateInAngles(float angles)
        {
            _desiredRotation = angles;
        }

        // Update is called once per frame
        void Update()
        {
            if(!_player.IsEdgeGrabbing())
                transform.Rotate(Vector3.up, _desiredRotation * _player.Sensibility  * _rotationMultiplier * Time.deltaTime);
        }
    }
}
