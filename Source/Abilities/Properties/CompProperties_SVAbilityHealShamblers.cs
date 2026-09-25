using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityHealShamblers : CompProperties_SVAbilityMark
    {
        public float radius = 6.9f;
        public float healAmount = 12f;
        public int maxWoundsPerPawn = 3;
        public FleckDef targetFleck;
        public FleckDef areaFleck;
        public float areaFleckScale = 1.4f;
        public SoundDef castSound;

        public CompProperties_SVAbilityHealShamblers()
        {
            compClass = typeof(CompAbilityEffect_SVHealShamblers);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (healAmount <= 0f && applyHediff == null)
            {
                yield return "SV heal shamblers needs a healAmount above 0 or an applyHediff";
            }
        }
    }
}
