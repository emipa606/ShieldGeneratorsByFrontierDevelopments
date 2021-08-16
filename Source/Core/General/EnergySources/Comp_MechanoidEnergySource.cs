using RimWorld;
using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class Comp_MechanoidEnergySource : Comp_RechargingEnergySource
    {
        protected override string SaveKey => "MechanoidSource";

        private CompProperties_MechanoidEnergySource Props => (CompProperties_MechanoidEnergySource) props;
        private float PowerGeneration => ((Pawn) parent).health.capacities.GetLevel(PawnCapacityDefOf.BloodPumping);

        protected override float ChargeRate => PowerGeneration * Props.rate;
    }
}