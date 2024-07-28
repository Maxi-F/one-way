using System;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Tooltip("Sounds that have a music song")]
        [SerializeField] private Sound[] musics;
        
        [Tooltip("Sounds that are SFX")]
        [SerializeField] private Sound[] sfxs;
        
        [Tooltip("Initial song that starts playing in game")]
        [SerializeField] private string initMusic;
        
        [Header("Settings")]
        [SerializeField] private PlayerSettingsConfig playerSettingsConfig;
        
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            // checks with null because if object is destroyed it returns true but object is not null.
            get { return _instance == null ? null : _instance; }
            private set { _instance = value; }
        }
        
        private Stack<List<Sound>> _pauseStack;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            } else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            _pauseStack = new Stack<List<Sound>>();
            
            foreach (Sound sound in musics)
            {
                sound.GenerateAudioSource(gameObject.AddComponent<AudioSource>());
                sound.SetVolume(playerSettingsConfig.musicVolume);
            }
            
            foreach (var sound in sfxs)
            {
                sound.GenerateAudioSource(gameObject.AddComponent<AudioSource>());
                sound.SetVolume(playerSettingsConfig.sfxVolume);
            }
        }

        private void Start()
        {
            PlayMusic(initMusic);
        }

        /// <summary>
        /// Play a Sound
        /// </summary>
        /// <param name="soundName">sound name to search for</param>
        /// <param name="sounds">List of sounds to search from</param>
        private void Play(string soundName, Sound[] sounds)
        {
            Sound soundToPlay = FindSound(soundName, sounds);

            if (soundToPlay == null) return;
            
            soundToPlay.Play();
        }
        
        /// <summary>
        /// Stop a Sound
        /// </summary>
        /// <param name="soundName">sound name to search for</param>
        /// <param name="sounds">List of sounds to search from</param>
        private void Stop(string soundName, Sound[] sounds)
        {
            Sound soundToStop = FindSound(soundName, sounds);
            if (soundToStop == null) return;

            soundToStop.Stop();
        }

        /// <summary>
        /// Pause all sounds
        /// </summary>
        /// <param name="sounds">List of sounds to pause</param>
        private void PauseAll(Sound[] sounds)
        {
            List<Sound> _playingSounds = new List<Sound>(); 
            foreach (Sound sound in sounds)
            {
                if (sound.IsPlaying())
                {
                    _playingSounds.Add(sound);
                }
                sound.Pause();
            }
            _pauseStack.Push(_playingSounds);
        }
        
        /// <summary>
        /// Check if a sound name is playing
        /// </summary>
        /// <param name="soundName">Name of the sound to search</param>
        /// <param name="sounds">List of sounds to search from</param>
        /// <returns></returns>
        private bool IsPlaying(string soundName, Sound[] sounds)
        {
            Sound sound = FindSound(soundName, sounds);

            return sound != null && sound.IsPlaying();
        }

        /// <summary>
        /// Set the volume for a list of sounds
        /// </summary>
        /// <param name="sounds">Sounds to set volume to</param>
        /// <param name="volume">the new volume for the sounds</param>
        private void SetVolumeFor(Sound[] sounds, float volume)
        {
            foreach (Sound sound in sounds)
            {
                sound.SetVolume(volume);
            }
        }

        /// <summary>
        /// Set the volume for all the musics
        /// </summary>
        /// <param name="volume">new volume</param>
        public void SetVolumeForMusic(float volume)
        {
            SetVolumeFor(musics, volume);
        }

        /// <summary>
        /// Set the volume for all SFXs
        /// </summary>
        /// <param name="volume">new volume</param>
        public void SetVolumeForSounds(float volume)
        {
            SetVolumeFor(sfxs, volume);
        }
        
        /// <summary>
        /// Play a music sound
        /// </summary>
        /// <param name="soundName">music name</param>
        public void PlayMusic(string soundName)
        { 
            Play(soundName, musics);
        }
        
        /// <summary>
        /// Play a SFX sound
        /// </summary>
        /// <param name="soundName">SFX name</param>
        public void PlaySound(string soundName)
        {
            Play(soundName, sfxs);
        }

        public void PlaySoundIfAble(string soundName)
        {
            Sound soundToPlay = FindSound(soundName, sfxs);

            if (soundToPlay.IsPlaying()) return;
            
            PlaySound(soundName);
        }

        /// <summary>
        /// Stop a SFX sound
        /// </summary>
        /// <param name="soundName">SFX name</param>
        public void StopSound(string soundName)
        {
            Stop(soundName, sfxs);
        }

        /// <summary>
        /// Pause all SFX sounds
        /// </summary>
        public void PauseAllSounds()
        {
            PauseAll(sfxs);
        }

        /// <summary>
        /// Resume all sounds
        /// </summary>
        public void ResumeAll()
        {
            List<Sound> soundsToPlay = _pauseStack.Pop();

            soundsToPlay.ForEach(sound => sound.Play());
            soundsToPlay.Clear();
        }

        /// <summary>
        /// Check if a SFX sound is playing
        /// </summary>
        /// <param name="soundName"></param>
        /// <returns></returns>
        public bool IsPlayingSound(string soundName)
        {
            return IsPlaying(soundName, sfxs);
        }
        
        /// <summary>
        /// Find a sound by its name, on a sound array
        /// </summary>
        /// <param name="soundName">a Sound name</param>
        /// <param name="soundArray">list of sounds to search from</param>
        /// <returns>The sound found, or null.</returns>
        private Sound FindSound (string soundName, Sound[] soundArray)
        {
            Sound foundSound = Array.Find(soundArray, sound => sound.soundName == soundName);
         
            if (foundSound == null)
            {
                Debug.LogWarning($"Sound {soundName} does not exist");
                return null;
            }

            return foundSound;
        }
    }
}