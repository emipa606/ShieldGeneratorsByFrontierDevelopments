using FrontierDevelopments.Shields.Comps;

namespace FrontierDevelopments.Shields.Linear
{
    public class CompProperties_ShieldLinear : CompProperties_ShieldBase
    {
        public int efficientRange = 0;
        public int maximumLinks = 2;

        public CompProperties_ShieldLinear()
        {
            compClass = typeof(Comp_ShieldLinear);
        }
    }
}