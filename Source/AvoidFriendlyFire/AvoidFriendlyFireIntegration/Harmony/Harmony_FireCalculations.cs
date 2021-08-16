using System.Collections.Generic;
using System.Linq;
using AvoidFriendlyFire;
using FrontierDevelopments.General;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace FrontierDevelopments.Shields.AvoidFriendlyFireIntegration.Harmony
{
    public class Harmony_FireCalculations
    {
        private static bool IsCellShielded(IntVec3 origin, int cellIndex, Map map, IEnumerable<IShieldField> shields)
        {
            return new FieldQuery(shields, map)
                .IsActive()
                .Intersects(
                    PositionUtility.ToVector3WithY(origin, 0),
                    PositionUtility.ToVector3WithY(map.cellIndices.IndexToCell(cellIndex), 0))
                .Get()
                .Any();
        }

        [HarmonyPatch(typeof(FireCalculations), "GetShootablePointsBetween")]
        [UsedImplicitly]
        private static class Patch_FireCalculations_old
        {
            [HarmonyPostfix]
            private static IEnumerable<int> AddShieldCheck(IEnumerable<int> results, IntVec3 origin,
                Map map)
            {
                if (Mod.Settings.EnableAIVerbFindShotLine)
                {
                    var fields = ShieldManager.For(map).Fields.ToList();

                    foreach (var cellIndex in results)
                    {
                        if (!IsCellShielded(origin, cellIndex, map, fields))
                        {
                            yield return cellIndex;
                        }
                        else
                        {
                            yield break;
                        }
                    }
                }
                else
                {
                    foreach (var cellIndex in results)
                    {
                        yield return cellIndex;
                    }
                }
            }
        }
    }
}