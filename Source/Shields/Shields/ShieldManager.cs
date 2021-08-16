using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace FrontierDevelopments.Shields
{
    public class ShieldManager : GameComponent
    {
        private readonly Dictionary<Map, IShieldManager> _managersByMap = new Dictionary<Map, IShieldManager>();
        private HashSet<IShieldManager> _managers = new HashSet<IShieldManager>();

        public ShieldManager()
        {
        }

        public ShieldManager(Game game)
        {
        }

        private static ShieldManager Component => Current.Game.GetComponent<ShieldManager>();

        public static IShieldManager For(Map map, bool create = true)
        {
            var found = Component._managersByMap.TryGetValue(map, out var result);
            if (found || !create)
            {
                return result;
            }

            result = new ShieldMapManager(map);
            Component._managers.Add(result);
            Component._managersByMap.Add(map, result);

            return result;
        }

        public static IShieldManager For(IShield shield)
        {
            return For(shield.Thing.Map, false);
        }

        public static IShieldManager For(IShieldField shield)
        {
            return For(shield.Map, false);
        }

        public static void Register(Map map, IShieldManager manager)
        {
            var component = Component;

            var existing = component._managersByMap.TryGetValue(map, out var result);
            if (existing)
            {
                component._managers.Remove(result);
            }

            component._managersByMap.Add(map, manager);
            component._managers.Add(manager);
        }

        public override void GameComponentTick()
        {
            _managers.Do(manager => manager.Tick());
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref _managers, "managers", LookMode.Deep);
            _managers?.Remove(null);
        }
    }
}