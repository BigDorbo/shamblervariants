using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityScream : CompProperties_SVAbilityMark
    {
        public MentalStateDef mentalState;
        public EffecterDef blastEffecter;
        public SoundDef screamSound;

        public CompProperties_SVAbilityScream()
        {
            compClass = typeof(CompAbilityEffect_SVScream);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (mentalState == null && applyHediff == null)
            {
                yield return "SV scream needs a mentalState or an applyHediff";
            }
        }
    }
}
