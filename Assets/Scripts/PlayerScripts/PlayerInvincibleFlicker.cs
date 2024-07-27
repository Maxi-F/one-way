using System;
using System.Collections;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInvincibleFlicker : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material transparentMaterial;
        [SerializeField] private Renderer playerRenderer;
        [SerializeField] private float flickSeconds = 0.5f;
        
        private Coroutine _flickCoroutine;
        private bool _isFlicking;
        private bool _hasNormalMaterial = true;
        
        private void Update()
        {
            if (player.IsInvincible && !_isFlicking)
            {
                StartCoroutine(Flick());
                _isFlicking = true;
            }
        }

        private IEnumerator Flick()
        {
            while (player.IsInvincible)
            {
                Debug.Log("Flicking!");
                playerRenderer.material = _hasNormalMaterial ? transparentMaterial : normalMaterial;

                _hasNormalMaterial = !_hasNormalMaterial;
                yield return new WaitForSeconds(flickSeconds);
            }

            playerRenderer.material = normalMaterial;
            _hasNormalMaterial = true;
            _isFlicking = false;
        }
    }
}
