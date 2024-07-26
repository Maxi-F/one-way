using UnityEngine;

namespace Enemies.EnemyInCrowd
{
    public class EnemyInCrowdAnimation : MonoBehaviour
    {
        [SerializeField] private Animator enemyInCrowdAnimator;

        public void HandleThrow(bool isThrowing)
        {
            enemyInCrowdAnimator.SetBool("isThrowing", isThrowing);
        }
    }
}
