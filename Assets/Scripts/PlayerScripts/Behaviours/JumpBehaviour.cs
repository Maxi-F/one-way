using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBehaviour : MonoBehaviour, IBehaviour
    {
        private JumpController _jumpController;
        private MoveController _moveController;

        public void Start()
        {
            _jumpController ??= GetComponent<JumpController>();
            _moveController ??= GetComponent<MoveController>();
        }
        public void Enter(IBehaviour previousBehaviour)
        {
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

        }

        public void Exit(IBehaviour nextBehaviour) {
            _jumpController.IsJumping = false;
        }

        public MovementBehaviour[] GetNextBehaviours()
        {
            MovementBehaviour[] nextBehaviours = { MovementBehaviour.Move, MovementBehaviour.EdgeGrab, MovementBehaviour.Fly };

            return nextBehaviours;
        }
    }
}
