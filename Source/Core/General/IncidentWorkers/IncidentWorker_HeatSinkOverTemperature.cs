﻿using System.Collections.Generic;
using System.Linq;
using FrontierDevelopments.General.Comps;
using RimWorld;
using Verse;

namespace FrontierDevelopments.General.IncidentWorkers
{
    public class IncidentWorker_HeatSinkOverTemperature : IncidentWorker
    {
        private static IEnumerable<Comp_HeatSink> GetTargets(Map map)
        {
            foreach (var thing in map.listerThings.AllThings)
            {
                var heatSink = thing.TryGetComp<Comp_HeatSink>();
                if (heatSink is {CanBreakdown: true})
                {
                    yield return heatSink;
                }
            }
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return Settings.EnableThermal
                   && GetTargets((Map) parms.target).Any();
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map) parms.target;
            var target = GetTargets(map).RandomElement();

            if (Settings.EnableCriticalThermalIncidents && target.OverCriticalThreshold)
            {
                target.DoCriticalBreakdown();
                return true;
            }

            if (Settings.EnableMajorThermalIncidents && target.OverMajorThreshold)
            {
                target.DoMajorBreakdown();
                return true;
            }

            if (!Settings.EnableMinorThermalIncidents || !target.OverMinorThreshold)
            {
                return false;
            }

            target.DoMinorBreakdown();
            return true;
        }
    }
}