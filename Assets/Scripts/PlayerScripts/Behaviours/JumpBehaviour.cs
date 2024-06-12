using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBehaviour : MonoBehaviour, IBehaviour
    {
        private JumpController _jumpController;
        private MoveController _moveController;

        public void Enter(IBehaviour previousBehaviour)
        {
            _jumpController ??= GetComponent<JumpController>();
            _moveController ??= GetComponent<MoveController>();
        }

        public void OnBehaviourFixedUpdate()
        {
            _jumpController.OnFixedUpdate();
            _moveController.MoveInAir();
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
            MovementBehaviour[] nextBehaviours = { MovementBehaviour.Move, MovementBehaviour.EdgeGrab };

            return nextBehaviours;
        }
    }
}
