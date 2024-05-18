using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collisioning with {other.gameObject.name}");
    }
}
