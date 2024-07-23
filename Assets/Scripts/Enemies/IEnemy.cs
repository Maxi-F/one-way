using PlayerScripts;

namespace Enemies
{
    public interface IEnemy
    {
        void SetPlayer(Player player);
        void Reset();
    }
}
