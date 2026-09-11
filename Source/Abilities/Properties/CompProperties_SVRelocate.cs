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
    }
}
