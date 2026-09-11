using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityPrime : CompProperties_AbilityEffect
    {
        public HediffDef primedHediff;
        public float requiredHostileRadius = 1.5f;

        public CompProperties_SVAbilityPrime()
        {
            compClass = typeof(CompAbilityEffect_SVPrime);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (primedHediff == null)
            {
                yield return "SV prime needs a primedHediff";
            }
        }
    }
}
