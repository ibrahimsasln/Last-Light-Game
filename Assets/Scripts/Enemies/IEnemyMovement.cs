using LastLight.Combat;

namespace LastLight.Enemies
{
    public interface IEnemyMovement
    {
        void Initialize(Health target, float speedMultiplier);
    }
}
