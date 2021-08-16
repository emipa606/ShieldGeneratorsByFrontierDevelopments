using Verse.AI;

namespace FrontierDevelopments.Shields
{
    public interface IShieldParent : IShieldWithStatus, IShieldUserInterface, IAttackTarget
    {
        bool ParentActive { get; }
        float SinkDamage(float damage);
    }
}