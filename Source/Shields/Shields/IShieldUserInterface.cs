using System.Collections.Generic;
using FrontierDevelopments.General.UI;
using Verse;

namespace FrontierDevelopments.Shields
{
    public interface IShieldUserInterface : IShieldSettable
    {
        IEnumerable<Gizmo> ShieldGizmos { get; }
        IEnumerable<UiComponent> UiComponents { get; }
    }
}