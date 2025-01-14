using System;
using System.Collections.Generic;
using System.Linq;
using FrontierDevelopments.General;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace FrontierDevelopments.Shields.Harmony
{
    public class Harmony_Verb
    {
        private static readonly List<Type> UncheckedTypes = new List<Type>();

        public static void BlacklistType(Type type)
        {
            UncheckedTypes.Add(type);
        }

        protected static bool ShieldBlocks(Thing caster, Verb verb, IntVec3 source, LocalTargetInfo target)
        {
            if (!Mod.Settings.EnableAIVerbFindShotLine)
            {
                return false;
            }

            if (!verb.verbProps.requireLineOfSight)
            {
                return false;
            }

            if (UncheckedTypes.Exists(a => a.IsInstanceOfType(verb)))
            {
                return false;
            }

            var friendlyShieldBlocks = new FieldQuery(caster.Map)
                .IsActive()
                .FriendlyTo(caster.Faction)
                .Intersects(
                    PositionUtility.ToVector3(source),
                    PositionUtility.ToVector3(target.Cell))
                .Get()
                .Any();
            return friendlyShieldBlocks;
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.TryFindShootLineFromTo))]
        [UsedImplicitly]
        private static class Patch_Verb_TryFindShootLineFromTo
        {
            [HarmonyPrefix]
            private static bool AddShieldCheck(ref bool __result, Verb __instance, IntVec3 root, LocalTargetInfo targ,
                ref ShootLine resultingLine)
            {
                if (!ShieldBlocks(__instance.caster, __instance, root, targ))
                {
                    return true;
                }

                __result = false;
                resultingLine = new ShootLine();
                return false;
            }
        }
    }
}