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
        [SerializeField] private JumpCoinConfig config;

        private string _noteSound;
        
        public void Start()
        {
            RemoveDebug();
        
            JumpCoinFactory factory = new JumpCoinFactory(config, FindObjectOfType<Player>());
            
            factory.CreateJumpCoin(gameObject);

            _noteSound = config.sounds[Random.Range(0, config.sounds.Count - 1)];
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
    }
}
