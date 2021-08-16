using FrontierDevelopments.General.Comps;

namespace FrontierDevelopments.ClimateControl
{
    public class CompProperties_ClimateControlHeatsink : CompProperties_HeatSink
    {
        public CompProperties_ClimateControlHeatsink()
        {
            compClass = typeof(Comp_ClimateControlHeatsink);
        }
    }
}