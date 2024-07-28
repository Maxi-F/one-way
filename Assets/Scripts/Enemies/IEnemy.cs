using Health;
using PlayerScripts;

namespace Enemies
{
    public interface IEnemy
    {
        /// <summary>
        /// Sets the player entity on the enemy, to make the enemy follow or look at for example.
        /// </summary>
        void SetPlayer(Player player);
        
        /// <summary>
        /// Resets the enemy position and enables it.
        /// </summary>
        void ResetEnemy();
    }
}
