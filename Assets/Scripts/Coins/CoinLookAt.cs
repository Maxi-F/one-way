using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLookAt : MonoBehaviour
{
    private Transform _lookToTransform;
    private float _yRotation;

    private void Start()
    {
        _yRotation = transform.eulerAngles.y;
    }

    public void SetTransform(Transform lookTo)
    {
        _lookToTransform = lookTo;
    }
    void Update()
    {
        Vector3 lookToPosition = new Vector3(_lookToTransform.position.x, transform.position.y, _lookToTransform.position.z);
        transform.LookAt(lookToPosition);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _yRotation, transform.eulerAngles.z);
    }
}
