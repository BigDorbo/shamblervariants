using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityBurrowJump : CompProperties_SVRelocate
    {
        public SoundDef breakSound;

        public CompProperties_SVAbilityBurrowJump()
        {
            compClass = typeof(CompAbilityEffect_SVBurrowJump);
            minRadius = 7.9f;
        }
    }
}
