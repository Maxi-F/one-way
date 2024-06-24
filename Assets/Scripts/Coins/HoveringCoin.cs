using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringCoin : MonoBehaviour
{
    [Header("Hover settings")]
    [SerializeField] private float hoverVelocity = 5f;
    [SerializeField] private float hoverDistance = 0.2f;

    private Vector3 _center;
    private int _actualDirection = 1;

    public void SetHoverSettings(float hoverVelocity, float hoverDistance)
    {
        this.hoverVelocity = hoverVelocity;
        this.hoverDistance = hoverDistance;
    }

    public void Start()
    {
        _center = transform.position;
    }

    public void Update()
    {
        float upLimit = _center.y + hoverDistance;
        float downLimit = _center.y - hoverDistance;

        if (upLimit < gameObject.transform.position.y)
        {
            _actualDirection = -1;
        }
        else if (downLimit > gameObject.transform.position.y)
        {
            _actualDirection = 1;
        }

        gameObject.transform.position += new Vector3(0, _actualDirection * hoverVelocity * Time.deltaTime, 0);
    }
}
