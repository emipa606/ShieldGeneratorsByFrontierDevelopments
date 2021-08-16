using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    public class ShieldQueryWithIntersects : IShieldQueryWithIntersects
    {
        private readonly IEnumerable<Pair<IShieldField, Vector3?>> _elements;
        private readonly Map _map;

        public ShieldQueryWithIntersects(IEnumerable<Pair<IShieldField, Vector3?>> elements, Map map)
        {
            _map = map;
            _elements = elements;
        }

        public ShieldQueryWithIntersects(IEnumerable<IShieldField> shields, Map map,
            Func<IShieldField, Vector3?> collision)
        {
            _map = map;
            _elements = Apply(shields, collision);
        }

        private IEnumerable<IShieldField> Shields => _elements.Select(e => e.First);

        public IShieldQueryWithIntersects IsActive(bool isActive = true)
        {
            return new ShieldQueryWithIntersects(
                _elements.Where(e => ShieldQueryUtils.IsActive(e.First, isActive)), _map);
        }

        public IShieldQueryWithIntersects OfFaction(Faction faction, bool invert = false)
        {
            return new ShieldQueryWithIntersects(
                _elements.Where(e => ShieldQueryUtils.OfFaction(e.First, faction, invert)), _map);
        }

        public IShieldQueryWithIntersects FriendlyTo(Faction faction, bool invert = false)
        {
            return new ShieldQueryWithIntersects(
                _elements.Where(e => ShieldQueryUtils.FriendlyTo(e.First, faction, invert)), _map);
        }

        public IShieldQueryWithIntersects HostileTo(Faction faction, bool invert = false)
        {
            return new ShieldQueryWithIntersects(
                _elements.Where(e => ShieldQueryUtils.HostileTo(e.First, faction, invert)), _map);
        }

        public IShieldQueryWithIntersects Intersects(Vector3 start, Vector3 end, bool invert = false)
        {
            return ShieldQueryUtils.Intersects(Shields, start, end, _map, invert);
        }

        public IShieldQueryWithIntersects Intersects(Vector3 position, bool invert = false)
        {
            return ShieldQueryUtils.Intersects(Shields, position, _map, invert);
        }

        public IEnumerable<IShieldField> Get()
        {
            return _elements.Select(e => e.First);
        }

        public IEnumerable<Pair<IShieldField, Vector3?>> GetWithIntersects()
        {
            return _elements;
        }

        public bool Block(ShieldDamages damages, Action<IShieldField, Vector3> onBlock)
        {
            try
            {
                var result = _elements
                    .Where(e => e.Second != null)
                    .First(e => e.First.Block(damages, e.Second.Value) >= damages.Damage);
                if (result.Second != null)
                {
                    onBlock?.Invoke(result.First, result.Second.Value);
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
            }

            return false;
        }

        public bool Block(float damage, Action<IShieldField, Vector3> onBlock)
        {
            try
            {
                var result = _elements
                    .Where(e => e.Second != null)
                    .First(e => e.First.Block(damage, e.Second.Value) >= damage);
                if (result.Second != null)
                {
                    onBlock?.Invoke(result.First, result.Second.Value);
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
            }

            return false;
        }

        [CanBeNull]
        public static IEnumerable<Pair<IShieldField, Vector3?>> Apply(IEnumerable<IShieldField> shields,
            Func<IShieldField, Vector3?> collision)
        {
            return shields.Select(shield => new Pair<IShieldField, Vector3?>(shield, collision(shield)));
        }
    }
}