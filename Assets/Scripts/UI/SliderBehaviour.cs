using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderBehaviour : MonoBehaviour
    {
        private Slider _slider;
    
        public void UseValue(float value)
        {
            _slider ??= GetComponent<Slider>();
            _slider.value = value;
        }
    }
}
