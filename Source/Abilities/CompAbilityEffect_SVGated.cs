using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompAbilityEffect_SVGated<T> : CompAbilityEffect_SV<T> where T : CompProperties_AbilityEffect
    {
        protected abstract bool ReadyFor(LocalTargetInfo target);

        protected abstract void Cast(LocalTargetInfo target, LocalTargetInfo dest);

        private bool Ready(LocalTargetInfo target)
        {
            return SVCast.CasterReady(parent.pawn) && ReadyFor(target);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return Ready(target) && base.Valid(target, throwMessages);
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return Ready(target);
        }

        public sealed override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (Ready(target))
            {
                Cast(target, dest);
            }
        }
    }
}
