using Tilemania.Combat;

namespace Tilemania.Enemies
{
    public interface IEnemyMovement
    {
        void Initialize(Health target, float speedMultiplier);
    }
}
