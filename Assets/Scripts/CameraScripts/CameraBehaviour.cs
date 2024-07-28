using System;
using System.Collections.Generic;
using Manager;
using PlayerScripts;
using UnityEngine;

namespace CameraScripts
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Player player;
    
        [Header("Follow Properties")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -5);
        [SerializeField] private float followSpeed = 5;

        [Header("Rotation properties")] 
        [Tooltip("Max Angles on the top of the target in which the camera can rotate in X")] 
        [SerializeField] private Vector2 topMinMaxAngles = new Vector2(80.0f, 100.0f);
    
        [Tooltip("Max Angles on the bottom of the target in which the camera can rotate in X")] 
        [SerializeField] private Vector2 bottomMinMaxAngles = new Vector2(260.0f, 280.0f);

        [SerializeField] private float controllerDamper = 0.05f;

        [Header("Events")] [SerializeField] private string invertAxisEvent = "invertAxis";
        
        private Vector2 _desiredRotation;
        private bool _isController;
        private bool _invertedAxis = false;

        private void Start()
        {
            EventManager.Instance?.SubscribeTo(invertAxisEvent, SetAxis);
        }

        private void OnDisable()
        {
            EventManager.Instance?.UnsubscribeTo(invertAxisEvent, SetAxis);
        }

        private void SetAxis(Dictionary<string, object> message)
        {
            bool value = (bool)message["value"];

            _invertedAxis = value;
        }

        private void FixedUpdate()
        {
            ModifyPosition();
        }

        private void LateUpdate()
        {
            ModifyRotation();
        }

        /// <summary>
        /// Rotates the camera around the target setted as a serialized field,
        /// Taking into account if the rotation was made with controller or mouse.
        ///
        /// It also takes into account top and bottom max angles, so it doesn't do a 360 rotation in X.
        /// </summary>
        private void ModifyRotation()
        {
            Quaternion previousRotationInX = transform.rotation;
            Vector3 previousPositionInX = transform.position;
            float multiplier = _isController ? controllerDamper : Time.deltaTime;
            
            transform.RotateAround(target.position, transform.right, _desiredRotation.y * player.Sensibility * multiplier);

            float angleInX = transform.rotation.eulerAngles.x;
        
            if ((angleInX > topMinMaxAngles.x && angleInX < topMinMaxAngles.y) ||
                (angleInX > bottomMinMaxAngles.x && angleInX < bottomMinMaxAngles.y))
            {
                transform.rotation = previousRotationInX;
                transform.position = previousPositionInX;
            }
        
            transform.RotateAround(target.position, Vector3.up, _desiredRotation.x * player.Sensibility * multiplier);
        }
    
        /// <summary>
        /// Modifies the position of the camera, taking into account the offset
        /// from the target. 
        /// </summary>
        private void ModifyPosition()
        {
            var rotatedOffset = transform.rotation * offset;
            var offsetEmulatingTransformPoint = target.position + rotatedOffset;

            transform.position = Vector3.Slerp(transform.position, offsetEmulatingTransformPoint, Time.fixedDeltaTime * followSpeed);
        }

        /// <summary>
        /// Sets the rotation desired direction.
        /// </summary>
        /// <param name="lookInput">Vector2 input of the controller/mouse</param>
        /// <param name="isController">flag to check if the input provided is from mouse or controller.</param>
        public void RotateCamera(Vector2 lookInput, bool isController)
        {
            _isController = isController;

            Vector2 lookInputToUse = lookInput;

            if (_invertedAxis)
            {
                lookInputToUse.y *= -1;
            }
            
            if (isController)
            {
                _desiredRotation = lookInputToUse;
            }
            else
            {
                _desiredRotation = new Vector2(lookInputToUse.x, -lookInputToUse.y);
            }
        }
    }
}
