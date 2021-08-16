using System.Reflection;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    [StaticConstructorOnStartup]
    public class Mod : Verse.Mod
    {
        private const string CentralizedClimateControlName = "Centralized Climate Control";
        public static string ModName;
        public static Settings Settings;

        public Mod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
            ModName = content.Name;

            var harmony = new HarmonyLib.Harmony("FrontierDevelopments.Shields");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory()
        {
            return "Frontier Developments Shields";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoWindowContents(inRect);
        }

        public static void SetDropCellCheckEnabled(bool enabled)
        {
            // intentionally do nothing, patched with harmony in the version specific extension of the mod
        }
    }
}