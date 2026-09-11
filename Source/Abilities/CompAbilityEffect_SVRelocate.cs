using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompAbilityEffect_SVRelocate<T> : CompAbilityEffect_SVGated<T> where T : CompProperties_SVRelocate
    {

        protected virtual bool ExtraReady(Pawn p)
        {
            return true;
        }

        protected bool Ready()
        {
            Pawn p = parent.pawn;
            if (!SVCast.CasterReady(p) || !ExtraReady(p))
            {
                return false;
            }
            Pawn prey = SVScan.NearestHostile(p, Props.searchRadius);
            return prey != null && !SVScan.WithinSquared(p.Position, prey.Position, Props.minRadius);
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            return Ready();
        }
    }
}
