using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class BatteryEnergySourceProperties : CompProperties
    {
        public float rate = float.PositiveInfinity;

        public BatteryEnergySourceProperties()
        {
            compClass = typeof(BatteryEnergySource);
        }
    }
}