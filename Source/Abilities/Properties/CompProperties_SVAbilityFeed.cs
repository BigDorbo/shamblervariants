using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityFeed : CompProperties_AbilityEffect
    {
        public HediffDef markHediff;
        public float bloodLoss = 0.28f;
        public float healAmount = 42f;
        public int maxWounds = 4;
        public int stunTicks = 120;
        public int filthCount = 8;
        public float filthRadius = 1.9f;
        public ThingDef filthDef;
        public SoundDef biteSound;

        public CompProperties_SVAbilityFeed()
        {
            compClass = typeof(CompAbilityEffect_SVFeed);
        }
    }
}
