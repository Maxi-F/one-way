using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;

public class UICounterText : MonoBehaviour
{
    [SerializeField] private string initValueEvent;
    [SerializeField] private string updateValueEvent;
    [SerializeField] private string resetValueEvent;
    [SerializeField] private bool isIncrement = true;
    [SerializeField] private int initValue;
    
    private TextMeshProUGUI _textMesh;
    private int _actualValue;
    private int _maxValue;
    
    void OnEnable()
    {
        _textMesh ??= GetComponent<TextMeshProUGUI>();
        EventManager.Instance.SubscribeTo(initValueEvent, InitValue);
        EventManager.Instance.SubscribeTo(updateValueEvent, UpdateValue);
        EventManager.Instance.SubscribeTo(resetValueEvent, ResetValue);

        _actualValue = initValue;
        SetText();
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeTo(updateValueEvent, UpdateValue);
        EventManager.Instance.UnsubscribeTo(resetValueEvent, ResetValue);
        EventManager.Instance.UnsubscribeTo(initValueEvent, InitValue);
    }

    private void SetText()
    {
        _textMesh.text = $"{_actualValue} / {_maxValue}";
    }

    void InitValue(Dictionary<string, object> message)
    {
        _maxValue = (int)message["value"];
        _actualValue = initValue;
        
        SetText();
    }
    
    void UpdateValue(Dictionary<string, object> message)
    {
        _actualValue = isIncrement ? _actualValue + 1 : _actualValue - 1;
        
        SetText();
    }

    void ResetValue(Dictionary<string, object> message)
    {
        _actualValue = initValue;
        
        SetText();
    }
}
