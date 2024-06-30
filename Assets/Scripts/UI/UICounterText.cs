using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
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

            if (initValueEvent != null)
            {
                EventManager.Instance?.SubscribeTo(initValueEvent, InitValue);
            }

            if (updateValueEvent != null)
            {
                EventManager.Instance?.SubscribeTo(updateValueEvent, UpdateValue);
            }

            if (resetValueEvent != null)
            {
                EventManager.Instance?.SubscribeTo(resetValueEvent, ResetValue);
            }

            _actualValue = initValue;
            SetText();
        }

        private void OnDisable()
        {
            if (initValueEvent != null)
            {
                EventManager.Instance?.UnsubscribeTo(initValueEvent, InitValue);
            }

            if (updateValueEvent != null)
            {
                EventManager.Instance?.UnsubscribeTo(updateValueEvent, UpdateValue);
            }

            if (resetValueEvent != null)
            {
                EventManager.Instance?.UnsubscribeTo(resetValueEvent, ResetValue);
            }
        }

        /// <summary>
        /// Sets the text of the counter UI.
        /// </summary>
        private void SetText()
        {
            _textMesh.text = $"{_actualValue} / {_maxValue}";
        }

        /// <summary>
        /// Inits counters.
        /// </summary>
        void InitValue(Dictionary<string, object> message)
        {
            _maxValue = (int)message["value"];
            _actualValue = initValue;
        
            SetText();
        }
    
        /// <summary>
        /// Updates counters.
        /// </summary>
        void UpdateValue(Dictionary<string, object> message)
        {
            _actualValue = isIncrement ? _actualValue + 1 : _actualValue - 1;
        
            SetText();
        }

        /// <summary>
        /// Resets counters.
        /// </summary>
        void ResetValue(Dictionary<string, object> message)
        {
            _actualValue = initValue;
        
            SetText();
        }
    }
}
