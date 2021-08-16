using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    [StaticConstructorOnStartup]
    public static class Resources
    {
        public static readonly Material ShieldMat =
            MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);

        // public static readonly Material ShieldPylon = MaterialPool.MatFrom("Things/Buildings/ShieldPylon", ShaderDatabase.Transparent);
        public static readonly Material SolidLine =
            MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Mote, Color.white);

        public static readonly Texture2D UiThermalShutoff = ContentFinder<Texture2D>.Get("UI/Buttons/ThermalShutoff");
        public static readonly Texture2D UiSetRadius = ContentFinder<Texture2D>.Get("UI/Buttons/Radius");

        public static readonly Texture2D
            UiChargeBattery = ContentFinder<Texture2D>.Get("UI/Buttons/PortableShieldDraw");

        public static readonly Texture2D UiToggleVisibility =
            ContentFinder<Texture2D>.Get("Other/ShieldBubble", ShaderDatabase.Transparent);
    }
}