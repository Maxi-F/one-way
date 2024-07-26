using UnityEngine;

namespace Enemies
{
    public class Tomato : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public void OnEnable()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void ThrowTo(Transform playerTransform)
        {
            Debug.Log(_rigidbody);
            
            Vector3 force = (playerTransform.position - transform.position).normalized * 10.0f;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
