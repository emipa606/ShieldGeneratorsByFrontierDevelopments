using FrontierDevelopments.General.Comps;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields.BadHygiene
{
    internal class Comp_BadHygieneHeatsink : Comp_HeatSink
    {
        private const float CapacityFactor = 1.5f;
        private const float CapacityEfficiencyFactor = 1.1f;

        private const float BaseCapacityRequired = 100f;

        private const float TargetTemp = 0f;

        private bool HasAirflow => (Vent?.Active ?? false) && Vent.WorkingNow;

        private Comp_DubsAirVent Vent { get; set; }

        private bool CapacityIsAvailable()
        {
            return Vent.PipeComp.pipeNet.CoolingCap - Vent.VentCapacity > 0;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Vent = parent.TryGetComp<Comp_DubsAirVent>();
        }

        protected override float AmbientTemp()
        {
            return HasAirflow ? CalculatedCoolingTemp() : base.AmbientTemp();
        }

        private float CalculatedCoolingTemp()
        {
            var capacityDifference = Vent.PipeComp.pipeNet.CoolingCap - Vent.VentCapacity;
            if (capacityDifference < 0)
            {
                return Mathf.Lerp(Temp, TargetTemp, Mathf.Pow(-capacityDifference, CapacityEfficiencyFactor) / 1000f);
            }

            return TargetTemp;
        }

        protected override void DissipateHeat(float kilojoules)
        {
            if (HasAirflow)
            {
                var additional = Mathf.Pow(Temp - TargetTemp, CapacityFactor);
                Vent.VentCapacity = BaseCapacityRequired + (additional > 0 ? additional : 0);
            }
            else
            {
                base.DissipateHeat(kilojoules);
            }
        }
    }
}