using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        private IBehaviour _behaviour;
        [FormerlySerializedAs("_rotationBehaviour")] [SerializeField] private RotationBehaviour rotationBehaviour;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private float groundedDistance = 0.1f;
        [SerializeField] Transform feetPivot;
        [SerializeField] private LayerMask floor;

        public void Awake()
        {
            if (rotationBehaviour == null)
            {
                Debug.LogError($"{name}: {nameof(rotationBehaviour)} is null!" +
                               $"\nThis class is dependant on a {nameof(rotationBehaviour)} component!");
            }
        }
        public void Start()
        {
            _behaviour ??= GetComponent<WalkingBehaviour>();
            rotationBehaviour ??= GetComponent<RotationBehaviour>();
        }

        public float GetBoxSize()
        {
            return capsuleCollider.radius;
        }

        public void SetBehaviour(IBehaviour newBehaviour)
        {
            Debug.Log($"{name}: Behaviour change to {newBehaviour.GetName()}");
            _behaviour = newBehaviour;
        }

        public bool IsRaycastOnFloor()
        {
            return Physics.Raycast(feetPivot.position, Vector3.down, out var hit, groundedDistance, floor);
        }
        
        public void Move(Vector3 direction)
        {
            _behaviour.Move(direction);
        }

        public void LookChange(Vector2 eulers)
        {
        
            _behaviour.LookChange();
            rotationBehaviour.RotateInAngles(eulers.x);
        }

        public void TouchesGround()
        {
            _behaviour.TouchesGround();
        }

        public void Jump()
        {
            _behaviour.Jump();
        }

        public void Update()
        {
            _behaviour.OnBehaviourUpdate();
        }

        public void FixedUpdate()
        {
            _behaviour.OnBehaviourFixedUpdate();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(feetPivot.position, Vector3.down * groundedDistance);
        }
    }
}
