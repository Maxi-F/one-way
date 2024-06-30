using System;
using System.Collections.Generic;
using Audio;
using Manager;
using PlayerScripts;
using ScriptableObjects.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coins.JumpCoin
{
    public class JumpCoin : WithDebugRemover
    {
        [Header("Coin config")]
        [SerializeField] private NoteConfig config;

        private string _noteSound;
        private GameObject _instantiatedNote;
        private Player _player;
        private JumpCoinFactory _jumpCoinFactory;
        
        public void Start()
        {
            _player = FindObjectOfType<Player>();
            _jumpCoinFactory = new JumpCoinFactory(config);

            if (_player == null)
            {
                Debug.LogError("Player not found from jump note!");
                return;
            }
            
            RemoveDebug();
            
            _jumpCoinFactory = new JumpCoinFactory(config);
            
            Enable();
        }

        void OnDisable()
        {
            JumpCoinPool.Instance.ReturnToPool(_instantiatedNote);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().AddJump();

                AudioManager.Instance.PlaySound(_noteSound);
                
                EventManager.Instance.TriggerEvent(
                    "coinObtained",
                    new Dictionary<string, object>()
                    {
                        { "gameObject", gameObject }
                    });
            
            
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Enables the jump coin, using a pooled note from the notes pool.
        /// </summary>
        public void Enable()
        {
            _instantiatedNote = JumpCoinPool.Instance.GetPooledNote();
            
            _jumpCoinFactory.Activate(_instantiatedNote, gameObject, _player.transform);

            _noteSound = config.sounds[Random.Range(0, config.sounds.Count - 1)];
            
            gameObject.SetActive(true);
        }
    }
}
