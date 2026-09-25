using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompProperties_SVRelocate : CompProperties_AbilityEffect
    {
        public float searchRadius = 24.9f;
        public float minRadius = 2.9f;
        public EffecterDef originEffecter;
        public EffecterDef arriveEffecter;

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (minRadius >= searchRadius)
            {
                yield return "SV relocate minRadius must be below searchRadius";
            }
        }
    }
}
