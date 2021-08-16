using System.Collections.Generic;
using CombatExtended;
using FrontierDevelopments.General;
using FrontierDevelopments.Shields.Harmony;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace FrontierDevelopments.CombatExtendedIntegration.Harmony
{
    public class Harmony_ExplosionCE : Harmony_Explosion
    {
        private static int GetDamage(ExplosionCE explosion, IntVec3 cell)
        {
            return explosion.GetDamageAmountAtCE(cell);
        }

        [HarmonyPatch(typeof(ExplosionCE), nameof(ExplosionCE.Tick))]
        [UsedImplicitly]
        private static class Patch_Tick
        {
            [HarmonyPrefix]
            private static void HandleOuterEdgesFirst(ExplosionCE __instance, int ___startTick,
                List<IntVec3> ___cellsToAffect)
            {
                HandleProtected(___cellsToAffect, __instance, __instance.TrueCenter(), ___startTick, GetDamage);
            }
        }

        [HarmonyPatch(typeof(ExplosionCE), "ExplosionCellsToHit", MethodType.Getter)]
        [UsedImplicitly]
        private static class Patch_AffectCell
        {
            [HarmonyPostfix]
            private static void CheckCellShielded(ExplosionCE __instance, ref IEnumerable<IntVec3> __result)
            {
                var returnList = new List<IntVec3>();
                foreach (var intVec3 in __result)
                {
                    if (TryBlock(__instance.Map, __instance.TrueCenter(), __instance.damType,
                        GetDamage(__instance, intVec3),
                        PositionUtility.ToVector3(intVec3)))
                    {
                        continue;
                    }

                    returnList.Add(intVec3);
                }

                __result = returnList;
            }
        }
    }
}