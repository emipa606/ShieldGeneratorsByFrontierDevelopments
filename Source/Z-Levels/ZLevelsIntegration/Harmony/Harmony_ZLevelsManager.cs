using HarmonyLib;
using JetBrains.Annotations;
using Verse;
using ZLevels;

namespace FrontierDevelopments.Shields.ZLevelsIntegration.Harmony
{
    public class Harmony_ZLevelsManager
    {
        [HarmonyPatch(typeof(ZLevelsManager), nameof(ZLevelsManager.CreateUpperLevel))]
        [UsedImplicitly]
        private static class Patch_CreateUpperLevel
        {
            [HarmonyPostfix]
            public static void AssociateShieldManager(Map __result, Map origin)
            {
                var manager = ShieldManager.For(origin, false);
                if (manager == null)
                {
                    return;
                }

                manager.AssociateWithMap(__result);
                ShieldManager.Register(__result, manager);
            }
        }
    }
}