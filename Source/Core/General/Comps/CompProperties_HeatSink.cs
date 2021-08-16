using Verse;

namespace FrontierDevelopments.General.Comps
{
    public class CompProperties_HeatSink : CompProperties
    {
        public float conductivity;
        public float criticalThreshold;
        public float grams;
        public float majorThreshold;
        public float maximumTemperature;

        public float minorThreshold;
        public float specificHeat;

        public CompProperties_HeatSink()
        {
            compClass = typeof(Comp_HeatSink);
        }
    }
}