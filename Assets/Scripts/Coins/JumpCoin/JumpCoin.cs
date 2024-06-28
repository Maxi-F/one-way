using System.Collections.Generic;
using Manager;
using PlayerScripts;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Coins.JumpCoin
{
    public class JumpCoin : WithDebugRemover
    {
        [Header("Coin config")]
        [SerializeField] private JumpCoinConfig config;
    
        public void Start()
        {
            RemoveDebug();
        
            JumpCoinFactory factory = new JumpCoinFactory(config, FindObjectOfType<Player>());

            factory.CreateJumpCoin(gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().AddJump();

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
