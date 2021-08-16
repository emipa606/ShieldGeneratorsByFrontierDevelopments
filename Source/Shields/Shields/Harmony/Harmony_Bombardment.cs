using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace FrontierDevelopments.Shields.Harmony
{
    internal class Harmony_Bombardment : Harmony_OrbitalStrike
    {
        [HarmonyPatch(typeof(Bombardment), "TryDoExplosion")]
        private static class Patch_TryDoExplosion
        {
            [HarmonyPrefix]
            private static bool Prefix_TryDoExplosion(Bombardment __instance, Bombardment.BombardmentProjectile proj)
            {
                return !ShouldStopHighDamage(proj.targetCell, __instance.Map);
            }
        }

        //[HarmonyPatch(typeof(Bombardment), "EffectTick")]
        //[UsedImplicitly]
        //private static class Patch_EffectTick
        //{
        //    [HarmonyTranspiler]
        //    private static IEnumerable<CodeInstruction> AddShieldCheck(
        //        IEnumerable<CodeInstruction> instructions,
        //        ILGenerator il)
        //    {
        //        return new Transpile_BlockBombardmentProjectile(il).Apply(instructions.ToList(), il);
        //    }

        //    // looking for:
        //    //
        //    //    IntVec3 targetCell = this.projectiles[index].targetCell;
        //    //    ...
        //    //    GenExplosion.DoExplosion(...);
        //    //    this.projectiles.RemoveAt(index);
        //    //
        //    //  Transform it into:
        //    //
        //    //    IntVec3 targetCell = this.projectiles[index].targetCell;
        //    //    if(!Harmony_Bombardment.ShouldStop(...)) {
        //    //      ...
        //    //      GenExplosion.DoExplosion(...);
        //    //    }
        //    //    this.projectiles.RemoveAt(index);
        //    private class Transpile_BlockBombardmentProjectile
        //    {
        //        private readonly Label _skipExplosion;
        //        private bool _addedSkipExplosionLabel;

        //        private bool _targetCellFound;

        //        public Transpile_BlockBombardmentProjectile(ILGenerator il)
        //        {
        //            _skipExplosion = il.DefineLabel();
        //        }

        //        private bool Success => _targetCellFound && _addedSkipExplosionLabel;

        //        private IEnumerable<CodeInstruction> AddShieldCheck(IEnumerable<CodeInstruction> instructions,
        //            ILGenerator il)
        //        {
        //            var targetCell = il.DeclareLocal(typeof(IntVec3));

        //            foreach (var instruction in instructions)
        //            {
        //                if (instruction.opcode == OpCodes.Ldfld
        //                    && (FieldInfo) instruction.operand == AccessTools.Field(
        //                        typeof(Bombardment.BombardmentProjectile),
        //                        nameof(Bombardment.BombardmentProjectile.targetCell)))
        //                {
        //                    _targetCellFound = true;

        //                    yield return instruction;
        //                    yield return new CodeInstruction(OpCodes.Stloc, targetCell);

        //                    // the shield check
        //                    yield return new CodeInstruction(OpCodes.Ldloc, targetCell);
        //                    yield return new CodeInstruction(OpCodes.Ldarg_0);
        //                    yield return new CodeInstruction(OpCodes.Call,
        //                        AccessTools.Property(typeof(Bombardment), nameof(Bombardment.Map)).GetGetMethod());
        //                    yield return new CodeInstruction(
        //                        OpCodes.Call,
        //                        AccessTools.Method(
        //                            typeof(Harmony_OrbitalStrike),
        //                            nameof(ShouldStopHighDamage)));
        //                    yield return new CodeInstruction(OpCodes.Brtrue, _skipExplosion);

        //                    // restore the stack to its original state
        //                    yield return new CodeInstruction(OpCodes.Ldloc, targetCell);
        //                }
        //                else
        //                {
        //                    yield return instruction;
        //                }
        //            }
        //        }

        //        private IEnumerable<CodeInstruction> AddShieldCheckLabel(IEnumerable<CodeInstruction> instructions)
        //        {
        //            foreach (var instruction in instructions)
        //            {
        //                if (instruction.opcode == OpCodes.Call
        //                    && (MethodInfo) instruction.operand == AccessTools.Method(
        //                        typeof(GenExplosion),
        //                        nameof(GenExplosion.DoExplosion)))
        //                {
        //                    _addedSkipExplosionLabel = true;
        //                    yield return instruction;
        //                    yield return new CodeInstruction(OpCodes.Nop)
        //                    {
        //                        labels = new List<Label>(new[] {_skipExplosion})
        //                    };
        //                }
        //                else
        //                {
        //                    yield return instruction;
        //                }
        //            }
        //        }

        //        public IEnumerable<CodeInstruction> Apply(IReadOnlyCollection<CodeInstruction> instructions,
        //            ILGenerator il)
        //        {
        //            var patched = AddShieldCheck(AddShieldCheckLabel(instructions), il).ToList();
        //            if (Success)
        //            {
        //                return patched;
        //            }

        //            Log.Warning("Failed patching Bombardment.EffectTick, blocking bombardment projectiles is disabled");
        //            return instructions;
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(Bombardment), "StartRandomFire")]
        [UsedImplicitly]
        private static class Patch_StartRandomFire
        {
            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> AddShieldCheck(
                IEnumerable<CodeInstruction> instructions,
                ILGenerator il)
            {
                return new Patcher(il).Apply(instructions.ToList(), il);
            }

            // looking for:
            //
            //    IntVec3 targetCell = ...
            //    FireUtility.TryStartFireIn(targetCell, ...);
            //
            //  Transform it into:
            //    
            //    IntVec3 targetCell = ...
            //    if(Harmony_Bombardment.IsShielded(targetCell, ...) {
            //      FireUtility.TryStartFireIn(targetCell, ...);
            //    }
            private class Patcher
            {
                private readonly Label _skipFire;
                private bool _addedSkipFireLabel;

                private bool _targetCellFound;

                public Patcher(ILGenerator il)
                {
                    _skipFire = il.DefineLabel();
                }

                private bool Success => _targetCellFound && _addedSkipFireLabel;

                private IEnumerable<CodeInstruction> AddShieldCheck(IEnumerable<CodeInstruction> instructions,
                    ILGenerator il)
                {
                    var targetCell = il.DeclareLocal(typeof(IntVec3));

                    foreach (var instruction in instructions)
                    {
                        if (instruction.opcode == OpCodes.Call
                            && (MethodInfo) instruction.operand == AccessTools.Method(
                                typeof(GenCollection),
                                nameof(GenCollection.RandomElementByWeight),
                                generics: new[] {typeof(IntVec3)}))
                        {
                            _targetCellFound = true;

                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Stloc, targetCell);

                            // the shield check
                            yield return new CodeInstruction(OpCodes.Ldloc, targetCell);
                            yield return new CodeInstruction(OpCodes.Ldarg_0);
                            yield return new CodeInstruction(
                                OpCodes.Call,
                                AccessTools.Method(
                                    typeof(Harmony_OrbitalStrike),
                                    nameof(ShouldStopHighDamageWithSmokeExplosion)));
                            yield return new CodeInstruction(OpCodes.Brtrue, _skipFire);

                            // restore the stack to its original state
                            yield return new CodeInstruction(OpCodes.Ldloc, targetCell);
                        }
                        else
                        {
                            yield return instruction;
                        }
                    }
                }

                private IEnumerable<CodeInstruction> AddShieldCheckLabel(IEnumerable<CodeInstruction> instructions)
                {
                    foreach (var instruction in instructions)
                    {
                        // bool RimWorld.FireUtility::TryStartFireIn(valuetype Verse.IntVec3, class Verse.Map, float32)
                        if (instruction.opcode == OpCodes.Call
                            && (MethodInfo) instruction.operand == AccessTools.Method(
                                typeof(FireUtility),
                                nameof(FireUtility.TryStartFireIn)))
                        {
                            _addedSkipFireLabel = true;
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Nop)
                            {
                                labels = new List<Label>(new[] {_skipFire})
                            };
                        }
                        else
                        {
                            yield return instruction;
                        }
                    }
                }

                public IEnumerable<CodeInstruction> Apply(IReadOnlyCollection<CodeInstruction> instructions,
                    ILGenerator il)
                {
                    var patched = AddShieldCheck(AddShieldCheckLabel(instructions), il).ToList();
                    if (Success)
                    {
                        return patched;
                    }

                    Log.Warning("Failed patching Bombardment.StartRandomFire, blocking bombardment fires is disabled");
                    return instructions;
                }
            }
        }
    }
}