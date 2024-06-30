using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;

        public bool loop = false;
    
        private AudioSource _source;
        
        public void GenerateAudioSource(AudioSource newSource)
        {
            _source = newSource;

            _source.clip = clip;
            _source.volume = volume;
            _source.pitch = pitch;
            _source.loop = loop;
        }
        
        public void Play()
        {
            _source.Play();
        }

        public void Pause()
        {
            _source.Pause();
        }

        public void Stop()
        {
            _source.Stop();
        }

        public bool IsPlaying()
        {
            return _source.isPlaying;
        }
    }
}
