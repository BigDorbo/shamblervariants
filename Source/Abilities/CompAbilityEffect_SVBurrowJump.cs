using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVBurrowJump : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityBurrowJump>
    {

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn p = parent.pawn;
            if (!Ready())
            {
                return;
            }
            IntVec3 cell;
            if (!SVCast.ResolveCell(p, target, dest, out cell))
            {
                return;
            }
            SVBurrow.BreakGround(p, Props.breakSound);
            SVCast.Teleport(p, cell, Props.originEffecter, Props.arriveEffecter);
            SVBurrow.BreakGround(p, Props.breakSound);
        }
    }
}
