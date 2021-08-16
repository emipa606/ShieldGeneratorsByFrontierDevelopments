using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    internal static class ShieldQueryUtils
    {
        public static bool IsActive(IShieldField shield, bool isActive = true)
        {
            return shield.IsActive() == isActive;
        }

        public static bool OfFaction(IShieldField shield, Faction faction, bool invert = false)
        {
            return shield.Faction == faction != invert;
        }

        public static bool FriendlyTo(IShieldField shield, Faction faction, bool invert = false)
        {
            return (shield.Faction == faction ||
                    shield.Faction.RelationKindWith(faction) == FactionRelationKind.Ally)
                   != invert;
        }

        public static bool HostileTo(IShieldField shield, Faction faction, bool invert = false)
        {
            return (shield.Faction != faction &&
                    shield.Faction.RelationKindWith(faction) == FactionRelationKind.Hostile)
                   != invert;
        }

        public static IShieldQueryWithIntersects Intersects(IEnumerable<IShieldField> shields, Vector3 start,
            Vector3 end, Map map, bool invert = false)
        {
            return new ShieldQueryWithIntersects(shields
                .Select(shield => new Pair<IShieldField, Vector3?>(shield, shield.Collision(start, end)))
                .Where(elements => elements.Second != null != invert), map);
        }

        public static IShieldQueryWithIntersects Intersects(IEnumerable<IShieldField> shields, Vector3 position,
            Map map, bool invert = false)
        {
            return new ShieldQueryWithIntersects(shields
                .Select(shield =>
                {
                    Vector3? point = null;
                    if (shield.Collision(position))
                    {
                        point = position;
                    }

                    return new Pair<IShieldField, Vector3?>(shield, point);
                })
                .Where(elements => elements.Second != null != invert), map);
        }
    }
}