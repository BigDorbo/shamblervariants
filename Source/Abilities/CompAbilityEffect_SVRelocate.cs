using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompAbilityEffect_SVRelocate<T> : CompAbilityEffect_SVGated<T> where T : CompProperties_SVRelocate
    {
        private Pawn prey;

        protected virtual bool ExtraReady(Pawn p)
        {
            return true;
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            Pawn p = parent.pawn;
            prey = ExtraReady(p) ? SVScan.NearestHostile(p, Props.searchRadius) : null;
            if (prey != null && p.Position.InHorDistOf(prey.Position, Props.minRadius))
            {
                prey = null;
            }
            return prey != null;
        }

        protected virtual bool PickCell(Pawn p, LocalTargetInfo target, LocalTargetInfo dest, out IntVec3 cell)
        {
            return SVCast.ResolveCell(p, target, dest, out cell);
        }

        protected abstract void Relocate(Pawn p, IntVec3 cell, Map map, Pawn prey);

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn p = parent.pawn;
            IntVec3 cell;
            if (PickCell(p, target, dest, out cell))
            {
                Relocate(p, cell, p.Map, prey);
            }
        }
    }
}
