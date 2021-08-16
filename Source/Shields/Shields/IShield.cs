using System.Collections.Generic;
using FrontierDevelopments.General;
using Verse;
using Verse.AI;

namespace FrontierDevelopments.Shields
{
    public interface IShield : IShieldWithStatus, IShieldUserInterface, ILabeled, IAttackTarget
    {
        IShieldParent Parent { get; }
        IEnumerable<IShieldField> Fields { get; }
        float DeploymentSize { get; }
        bool PresentOnMap(Map map);
        void SetParent(IShieldParent shieldParent);
        bool IsActive();
    }
}