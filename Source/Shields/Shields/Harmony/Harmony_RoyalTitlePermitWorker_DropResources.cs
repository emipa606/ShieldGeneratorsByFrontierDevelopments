using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace FrontierDevelopments.Shields.Harmony
{
    internal static class Harmony_RoyalTitlePermitWorker_DropResources
    {
        [HarmonyPatch]
        [UsedImplicitly]
        private static class Patch_BeginCallResources
        {
            [HarmonyTargetMethod]
            private static MethodInfo Target()
            {
                return typeof(RoyalTitlePermitWorker_DropResources)
                    .GetNestedTypes(BindingFlags.Instance | BindingFlags.NonPublic)
                    .SelectMany(AccessTools.GetDeclaredMethods)
                    .Where(method => method.Name.Contains("BeginCallResources"))
                    .Where(method => method.ReturnType == typeof(bool))
                    .First(method => method.GetParameters().Any(param => param.ParameterType == typeof(TargetInfo)));
            }

            [HarmonyPostfix]
            private static bool AddShieldCheck(bool result, TargetInfo target)
            {
                if (!result)
                {
                    return false;
                }

                var blocked = new FieldQuery(target.Map)
                    .Intersects(target.Cell.ToVector3().Yto0())
                    .Get()
                    .Any();
                if (blocked)
                {
                    return false;
                }

                return true;
            }
        }
    }
}