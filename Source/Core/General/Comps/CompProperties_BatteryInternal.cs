using RimWorld;

namespace FrontierDevelopments.General.Comps
{
    public class CompProperties_BatteryInternal : CompProperties_Battery
    {
        public float chargeRate;

        public CompProperties_BatteryInternal()
        {
            compClass = typeof(Comp_BatteryInternal);
        }
    }
}