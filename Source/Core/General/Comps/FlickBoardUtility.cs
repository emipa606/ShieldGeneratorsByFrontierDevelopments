using System.Collections.Generic;
using System.Linq;
using Verse;

namespace FrontierDevelopments.General.Comps
{
    public static class FlickBoardUtility
    {
        public static Comp_FlickBoard FindBoard(ThingWithComps thing)
        {
            return thing.GetComp<Comp_FlickBoard>();
        }

        public static IEnumerable<IFlickBoardSwitch> FindSwitches(Thing thing)
        {
            if (thing is IFlickBoardSwitch thingSwitch)
            {
                yield return thingSwitch;
            }

            if (thing is ThingWithComps thingWithComps)
            {
                foreach (var compSwitch in Find(thingWithComps.AllComps))
                {
                    yield return compSwitch;
                }
            }
        }

        public static IEnumerable<IFlickBoardSwitch> Find(IEnumerable<ThingComp> comps)
        {
            return comps.OfType<IFlickBoardSwitch>();
        }
    }
}