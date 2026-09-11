using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityHealShamblers : CompProperties_AbilityEffect
    {
        public float radius = 6.9f;
        public float healAmount = 12f;
        public int maxWoundsPerPawn = 3;
        public FleckDef targetFleck;
        public FleckDef areaFleck;
        public float areaFleckScale = 1.4f;
        public SoundDef castSound;
        public HediffDef applyHediff;
        public int hediffDurationTicks;

        public CompProperties_SVAbilityHealShamblers()
        {
            compClass = typeof(CompAbilityEffect_SVHealShamblers);
        }
    }
}
