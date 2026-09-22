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

        public CompProperties_SVAbilityRaiseCorpse()
        {
            compClass = typeof(CompAbilityEffect_SVRaiseCorpse);
        }
    }
}
