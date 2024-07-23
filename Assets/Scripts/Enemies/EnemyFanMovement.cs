using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class EnemyFanMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    
    [Header("Events")]
    [SerializeField] private string enemyFanEnabledEvent = "enemyFanEnabled";
    
    private Transform _playerTransform;
    void Start()
    {
        EventManager.Instance.TriggerEvent(enemyFanEnabledEvent, new Dictionary<string, object>()
        {
            { "enemyFan", this }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerTransform) return;

        Vector3 direction = (_playerTransform.position - transform.position).normalized;

        transform.position += direction * (speed * Time.deltaTime);
    }

    /// <summary>
    /// Sets the player transform.
    /// </summary>
    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
