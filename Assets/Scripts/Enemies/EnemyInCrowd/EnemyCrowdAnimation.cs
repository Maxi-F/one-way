using UnityEngine;

namespace Enemies.EnemyInCrowd
{
    public class EnemyCrowdAnimation : MonoBehaviour
    {
        [SerializeField] private Animator enemyInCrowdAnimator;

        public void HandleThrow(bool isThrowing)
        {
            enemyInCrowdAnimator.SetBool("isThrowing", isThrowing);
        }
    }
}
