using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] musics;
        [SerializeField] private Sound[] sfxs;
        [SerializeField] private string initMusic;
        
        [Header("Settings")]
        [SerializeField] private PlayerSettings playerSettings;
        
        public static AudioManager Instance;
        
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
                sound.SetVolume(playerSettings.musicVolume);
            }
            
            foreach (var sound in sfxs)
            {
                sound.GenerateAudioSource(gameObject.AddComponent<AudioSource>());
                sound.SetVolume(playerSettings.sfxVolume);
            }
        }

        private void Start()
        {
            PlayMusic(initMusic);
        }

        private void Play(string soundName, Sound[] sounds)
        {
            Sound soundToPlay = FindSound(soundName, sounds);

            if (soundToPlay == null) return;
            
            soundToPlay.Play();
        }
        
        private void Stop(string soundName, Sound[] sounds)
        {
            Sound soundToStop = FindSound(soundName, sounds);
            if (soundToStop == null) return;

            soundToStop.Stop();
        }

        private void StopAll(Sound[] sounds)
        {
            foreach (Sound sound in sounds)
            {
                sound.Stop();
            }
        }

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
        
        private bool IsPlaying(string name, Sound[] sounds)
        {
            Sound sound = FindSound(name, sounds);

            return sound != null && sound.IsPlaying();
        }

        private void SetVolumeFor(Sound[] sounds, float volume)
        {
            foreach (Sound sound in sounds)
            {
                sound.SetVolume(volume);
            }
        }

        public void SetVolumeForMusic(float volume)
        {
            SetVolumeFor(musics, volume);
        }

        public void SetVolumeForSounds(float volume)
        {
            SetVolumeFor(sfxs, volume);
        }
        
        public void PlayMusic(string soundName)
        { 
            Play(soundName, musics);
        }

        public void StopMusic(string soundName)
        {
            Stop(soundName, musics);
        }

        public void StopAllMusic()
        {
            StopAll(musics);
        }

        public void PauseAllMusics()
        {
            PauseAll(musics);
        }
        
        public void PlaySound(string soundName)
        {
            Play(soundName, sfxs);
        }

        public void StopSound(string soundName)
        {
            Stop(soundName, sfxs);
        }

        public void StopAllSounds()
        {
            StopAll(sfxs);
        }

        public void PauseAllSounds()
        {
            PauseAll(sfxs);
        }

        public void ResumeAll()
        {
            List<Sound> _soundsToPlay = _pauseStack.Pop();

            _soundsToPlay.ForEach(sound => sound.Play());
            _soundsToPlay.Clear();
        }

        public bool IsPlayingSound(string soundName)
        {
            return IsPlaying(soundName, sfxs);
        }

        public bool IsPlayingMusic(string musicName)
        {
            return IsPlaying(musicName, musics);
        }
        
        private Sound FindSound (string name, Sound[] soundArray)
        {
            Sound foundSound = Array.Find(soundArray, sound => sound.name == name);
         
            if (foundSound == null)
            {
                Debug.LogWarning($"Sound {name} does not exist");
                return null;
            }

            return foundSound;
        }
    }
}