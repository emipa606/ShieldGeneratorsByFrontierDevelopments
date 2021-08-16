using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    public class FieldQuery : IShieldQuery
    {
        private readonly Map _map;
        private readonly IEnumerable<IShieldField> _shields;

        public FieldQuery(IEnumerable<IShieldField> shields, Map map)
        {
            _map = map;
            _shields = shields.Where(shield => shield.PresentOnMap(map));
        }

        public FieldQuery(Map map)
        {
            _map = map;
            _shields = ShieldManager.For(map).Fields.Where(shield => shield.PresentOnMap(map));
        }

        public IShieldQuery IsActive(bool isActive = true)
        {
            return new FieldQuery(_shields.Where(shield => ShieldQueryUtils.IsActive(shield, isActive)), _map);
        }

        public IShieldQuery OfFaction(Faction faction, bool invert = false)
        {
            return new FieldQuery(_shields.Where(shield => ShieldQueryUtils.OfFaction(shield, faction, invert)), _map);
        }

        public IShieldQuery FriendlyTo(Faction faction, bool invert = false)
        {
            return new FieldQuery(_shields.Where(shield => ShieldQueryUtils.FriendlyTo(shield, faction, invert)), _map);
        }

        public IShieldQuery HostileTo(Faction faction, bool invert = false)
        {
            return new FieldQuery(_shields.Where(shield => ShieldQueryUtils.HostileTo(shield, faction, invert)), _map);
        }

        public IShieldQueryWithIntersects Intersects(Vector3 start, Vector3 end, bool invert = false)
        {
            return ShieldQueryUtils.Intersects(_shields, start, end, _map, invert);
        }

        public IShieldQueryWithIntersects Intersects(Vector3 position, bool invert = false)
        {
            return ShieldQueryUtils.Intersects(_shields, position, _map, invert);
        }

        public IEnumerable<IShieldField> Get()
        {
            return _shields;
        }
    }
}