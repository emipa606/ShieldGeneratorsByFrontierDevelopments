using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    public interface IShieldQueryWithIntersects
    {
        IShieldQueryWithIntersects IsActive(bool isActive = true);
        IShieldQueryWithIntersects OfFaction(Faction faction, bool invert = false);
        IShieldQueryWithIntersects FriendlyTo(Faction faction, bool invert = false);
        IShieldQueryWithIntersects HostileTo(Faction faction, bool invert = false);
        IShieldQueryWithIntersects Intersects(Vector3 start, Vector3 end, bool invert = false);
        IShieldQueryWithIntersects Intersects(Vector3 position, bool invert = false);
        IEnumerable<IShieldField> Get();
        IEnumerable<Pair<IShieldField, Vector3?>> GetWithIntersects();
        bool Block(ShieldDamages damages, Action<IShieldField, Vector3> onBlock = null);
        bool Block(float damage, Action<IShieldField, Vector3> onBlock = null);
    }
}