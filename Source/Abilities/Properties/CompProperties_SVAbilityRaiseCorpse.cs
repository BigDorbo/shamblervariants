using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityRaiseCorpse : CompProperties_AbilityEffect
    {
        public int cellsToFill = 10;
        public bool gravesOnly;
        public bool corpsesOnly;
        public bool exhumeOnly;
        public float digFleckScale = 2.2f;
        public float claimRadius = 6.9f;

        public CompProperties_SVAbilityRaiseCorpse()
        {
            compClass = typeof(CompAbilityEffect_SVRaiseCorpse);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (gravesOnly && corpsesOnly)
            {
                yield return "SV raise corpse cannot be both gravesOnly and corpsesOnly";
            }
            if (!exhumeOnly && cellsToFill <= 0)
            {
                yield return "SV raise corpse needs cellsToFill above 0";
            }
            if (!exhumeOnly && claimRadius <= 0f)
            {
                yield return "SV raise corpse needs a claimRadius above 0";
            }
        }
    }
}
