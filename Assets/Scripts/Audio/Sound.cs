using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    [Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float maxVolume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;
        
        public bool loop = false;
    
        private AudioSource _source;
        private float _volume;
        
        /// <summary>
        /// Sets all values of an Unity.AudioSource Component based on
        /// the sound's values.
        /// </summary>
        /// <param name="newSource">The AudioSource component to set values to.</param>
        public void GenerateAudioSource(AudioSource newSource)
        {
            _source = newSource;
            _volume = maxVolume;
            
            _source.clip = clip;
            _source.volume = _volume;
            _source.pitch = pitch;
            _source.loop = loop;
        }

        /// <summary>
        /// Changes the volume of the audio source.
        /// It takes into account the max volume that the sound can have.
        /// </summary>
        /// <param name="newVolume">New volume to set</param>
        public void SetVolume(float newVolume)
        {
            _volume = newVolume * maxVolume;
            _source.volume = _volume;
        }
        
        /// <summary>
        /// Plays the sound on the audio source.
        /// </summary>
        public void Play()
        {
            _source.Play();
        }

        /// <summary>
        /// Pauses the sound in the audio source.
        /// </summary>
        public void Pause()
        {
            _source.Pause();
        }

        /// <summary>
        /// Stops the sound in the audio source.
        /// </summary>
        public void Stop()
        {
            _source.Stop();
        }

        /// <summary>
        /// Checks if the audio source is playing or not.
        /// </summary>
        public bool IsPlaying()
        {
            return _source.isPlaying;
        }
    }
}
