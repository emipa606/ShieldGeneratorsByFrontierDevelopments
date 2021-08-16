using System.Collections.Generic;
using Verse;

namespace FrontierDevelopments.Shields.Comps
{
    public class CompProperties_ShieldResistance : CompProperties
    {
        public List<ShieldResistance> resists;

        public CompProperties_ShieldResistance()
        {
            compClass = typeof(Comp_ShieldResistance);
        }
    }
}