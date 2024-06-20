using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBehaviour : MonoBehaviour, IBehaviour
    {
        private JumpController _jumpController;
        private MoveController _moveController;
        private EdgeGrabController _edgeGrabController;

        public void Start()
        {
            _jumpController ??= GetComponent<JumpController>();
            _moveController ??= GetComponent<MoveController>();
            _edgeGrabController ??= GetComponent<EdgeGrabController>();
        }
        public void Enter(IBehaviour previousBehaviour)
        {
            if (previousBehaviour.GetName() == MovementBehaviour.Jump)
            {
                _jumpController.JumpFromAir();
            }
        }

        public void OnBehaviourFixedUpdate()
        {
            _jumpController.OnFixedUpdate();
            _moveController.MovePlayer();
        }

        public MovementBehaviour GetName()
        {
            return MovementBehaviour.Jump;
        }

        public void OnBehaviourUpdate()
        {
            _jumpController.OnUpdate();
            _edgeGrabController.CheckEdge();
        }

        public void Exit(IBehaviour nextBehaviour) {
            _jumpController.IsJumping = false;

            if (nextBehaviour.GetName() != MovementBehaviour.Jump)
            {
                _jumpController.ResetJumps();
            }
        }

        public MovementBehaviour[] GetNextBehaviours()
        {
            MovementBehaviour[] nextBehaviours = { MovementBehaviour.Move, MovementBehaviour.EdgeGrab, MovementBehaviour.Fly, MovementBehaviour.Jump };

            return nextBehaviours;
        }
    }
}
