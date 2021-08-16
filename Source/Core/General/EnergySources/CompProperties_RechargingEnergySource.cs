namespace FrontierDevelopments.General.EnergySources
{
    public class CompProperties_RechargingEnergySource : CompProperties_SingleUseEnergySource
    {
        public float minimum;
        public float onlineRechargeAmount;

        public CompProperties_RechargingEnergySource()
        {
            compClass = typeof(Comp_RechargingEnergySource);
        }
    }
}