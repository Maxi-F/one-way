using UnityEngine;

namespace PlayerScripts
{
    public class RotationBehaviour : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 1;
        private float _desiredRotation = 0;
    
        public void RotateInAngles(float angles)
        {
            _desiredRotation = angles;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, _desiredRotation * sensitivity * Time.deltaTime);
        }
    }
}
