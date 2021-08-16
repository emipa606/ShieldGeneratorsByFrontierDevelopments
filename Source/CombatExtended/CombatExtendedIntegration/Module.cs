using System.Reflection;
using CombatExtended;
using FrontierDevelopments.Shields.Harmony;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace FrontierDevelopments.CombatExtendedIntegration
{
    public class Module : Mod
    {
        public Module(ModContentPack content) : base(content)
        {
            Log.Message("Frontier Developments Shields :: Loading Combat Extended (Continued) support");

            var harmony = new HarmonyLib.Harmony("FrontierDevelopment.Shields.CombatExtended");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
        [UsedImplicitly]
        private static class Load
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                Harmony_Verb.BlacklistType(typeof(Verb_ShootMortarCE));
                Harmony_Verb.BlacklistType(typeof(Verb_MarkForArtillery));
            }
        }
    }
}