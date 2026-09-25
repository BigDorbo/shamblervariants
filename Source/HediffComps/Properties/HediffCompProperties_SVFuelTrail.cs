using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVFuelTrail : HediffCompProperties
    {
        public ThingDef filthDef;
        public int moveIntervalTicks = 45;
        public int idleIntervalTicks = 420;
        public bool onlyWhenEngaged;

        public HediffCompProperties_SVFuelTrail()
        {
            compClass = typeof(HediffComp_SVFuelTrail);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (filthDef == null)
            {
                yield return "SV fuel trail needs a filthDef";
            }
            if (moveIntervalTicks <= 0 || idleIntervalTicks <= 0)
            {
                yield return "SV fuel trail needs moveIntervalTicks and idleIntervalTicks above 0";
            }
        }
    }
}
