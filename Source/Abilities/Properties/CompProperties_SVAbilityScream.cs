using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityScream : CompProperties_AbilityEffect
    {
        public MentalStateDef mentalState;
        public HediffDef applyHediff;
        public int hediffDurationTicks;
        public EffecterDef blastEffecter;
        public SoundDef screamSound;

        public CompProperties_SVAbilityScream()
        {
            compClass = typeof(CompAbilityEffect_SVScream);
        }
    }
}
