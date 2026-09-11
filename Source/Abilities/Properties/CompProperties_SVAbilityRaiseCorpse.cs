using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityRaiseCorpse : CompProperties_AbilityEffect
    {
        public GasType gasType = GasType.DeadlifeDust;
        public int cellsToFill = 10;
        public bool gravesOnly;
        public bool corpsesOnly;
        public bool exhumeOnly;
        public FleckDef digFleck;
        public float digFleckScale = 2.2f;

        public CompProperties_SVAbilityRaiseCorpse()
        {
            compClass = typeof(CompAbilityEffect_SVRaiseCorpse);
        }
    }
}
