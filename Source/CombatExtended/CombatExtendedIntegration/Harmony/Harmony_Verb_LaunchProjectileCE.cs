using CombatExtended;
using FrontierDevelopments.Shields.Harmony;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace FrontierDevelopments.CombatExtendedIntegration.Harmony
{
    public class Harmony_Verb_LaunchProjectileCE : Harmony_Verb
    {
        [HarmonyPatch(typeof(Verb_LaunchProjectileCE), nameof(Verb_LaunchProjectileCE.CanHitTargetFrom),
            new[] {typeof(IntVec3), typeof(LocalTargetInfo), typeof(string)},
            new[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref})]
        [UsedImplicitly]
        private static class Patch_CanHitTargetFrom
        {
            [HarmonyPostfix]
            private static bool Postfix(
                bool __result,
                Verb_LaunchProjectileCE __instance,
                IntVec3 root,
                LocalTargetInfo targ,
                ref string report)
            {
                if (!__result)
                {
                    return false;
                }

                if (!ShieldBlocks(__instance.caster, __instance, root, targ))
                {
                    return true;
                }

                report = "Block by shield";
                return false;
            }
        }
    }
}