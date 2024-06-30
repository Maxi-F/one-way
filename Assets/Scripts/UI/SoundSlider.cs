using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum SoundType
    {
        Music,
        Sfx
    }

    public class SoundSlider : MonoBehaviour
    {
        [SerializeField] private SoundType soundType;
        
        private Slider _slider;
        
        private GameplayManager _gameplayManager;
        
        private void OnEnable()
        {
            _slider ??= GetComponent<Slider>();
            _gameplayManager ??= FindObjectOfType<GameplayManager>();

            switch (soundType)
            {
                case SoundType.Music:
                    _slider.value = _gameplayManager.GetMusicVolume();
                    break;
                case SoundType.Sfx:
                    _slider.value = _gameplayManager.GetSoundVolume();
                    break;
            }
        }
        
        public void OnValueChange(float value)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    AudioManager.Instance.SetVolumeForMusic(value);
                    _gameplayManager.SetMusicVolume(value);
                    break;
                case SoundType.Sfx:
                    AudioManager.Instance.SetVolumeForSounds(value);
                    _gameplayManager.SetSoundVolume(value);
                    break;
            }
            
            
        }
    }
}