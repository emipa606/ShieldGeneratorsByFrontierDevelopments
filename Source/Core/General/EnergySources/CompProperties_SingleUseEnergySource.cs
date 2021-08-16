using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class CompProperties_SingleUseEnergySource : CompProperties
    {
        public float charge;
        public float rate = float.PositiveInfinity;

        public CompProperties_SingleUseEnergySource()
        {
            compClass = typeof(Comp_SingleUseEnergySource);
        }
    }
}