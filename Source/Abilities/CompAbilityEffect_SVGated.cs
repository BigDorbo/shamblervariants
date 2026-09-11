using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompAbilityEffect_SVGated<T> : CompAbilityEffect_SV<T> where T : CompProperties_AbilityEffect
    {
        protected abstract bool ReadyFor(LocalTargetInfo target);

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return ReadyFor(target) && base.Valid(target, throwMessages);
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return ReadyFor(target);
        }
    }
}
