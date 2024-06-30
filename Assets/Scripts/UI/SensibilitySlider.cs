using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SensibilitySlider : MonoBehaviour
    { 
        [SerializeField] private string sensibilityChangedEvent = "sensibilityChanged";
        private Slider _slider;
        
        private GameplayManager _gameplayManager;
        
        private void OnEnable()
        {
            _slider ??= GetComponent<Slider>();
            _gameplayManager ??= FindObjectOfType<GameplayManager>();
            
            _slider.value = _gameplayManager.GetSensibility();
        }

        public void OnValueChange(float value)
        {
            EventManager.Instance.TriggerEvent(sensibilityChangedEvent, new Dictionary<string, object>() { { "value", value } });
        }
    }
}
