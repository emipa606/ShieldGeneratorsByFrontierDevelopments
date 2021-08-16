using FrontierDevelopments.General.Comps;

namespace FrontierDevelopments.Shields.BadHygiene
{
    public class CompProperties_BadHygieneHeatsink : CompProperties_HeatSink
    {
        public CompProperties_BadHygieneHeatsink()
        {
            compClass = typeof(Comp_BadHygieneHeatsink);
        }
    }
}