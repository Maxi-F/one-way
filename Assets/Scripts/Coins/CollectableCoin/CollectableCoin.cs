using Coins.JumpCoin;
using Manager;
using UnityEngine;

namespace Coins.CollectableCoin
{
    public class CollectableCoin : WithDebugRemover
    {
        [Header("Events")]
        [SerializeField] private string collectableCoinEvent = "collectableCoinObtained";
    
        private void Start()
        {
            RemoveDebug();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.Instance?.TriggerEvent(collectableCoinEvent, null);
            
                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
    }
}
