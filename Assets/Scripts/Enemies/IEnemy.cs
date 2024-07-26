using Health;
using PlayerScripts;

namespace Enemies
{
    public interface IEnemy : ITakeDamage
    {
        void SetPlayer(Player player);
        void ResetEnemy();
        void Dead();
    }
}
