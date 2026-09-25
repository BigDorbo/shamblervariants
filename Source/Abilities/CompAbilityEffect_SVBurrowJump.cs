using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVBurrowJump : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityBurrowJump>
    {
        protected override void Relocate(Pawn p, IntVec3 cell, Map map, Pawn prey)
        {
            BreakGround(p, map);
            SVCast.Teleport(p, cell, Props.originEffecter, Props.arriveEffecter);
            BreakGround(p, map);
        }

        private void BreakGround(Pawn p, Map map)
        {
            FleckMaker.ThrowDustPuff(p.Position, map, 2.2f);
            FleckMaker.ThrowDustPuff(p.Position, map, 1.6f);
            FleckMaker.ThrowDustPuff(p.Position, map, 1f);
            SVCast.Sound(Props.breakSound, p);
        }
    }
}
