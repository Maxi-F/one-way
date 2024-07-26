using System;
using UnityEngine;

namespace Enemies
{
    public class TomatoRotation : MonoBehaviour
    {
        [SerializeField] private float rotationVelocity = 10.0f;

        public void Update()
        {
            transform.Rotate(transform.right * (rotationVelocity * Time.deltaTime));
        }
    }
}
