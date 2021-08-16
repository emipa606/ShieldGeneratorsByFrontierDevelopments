using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class CompProperties_ElectricEnergySource : CompProperties
    {
        public float minimumOnlinePower;
        public float rate = float.PositiveInfinity;

        public CompProperties_ElectricEnergySource()
        {
            compClass = typeof(Comp_ElectricEnergySource);
        }
    }
}