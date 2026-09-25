using RimWorld;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVLeap : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityLeap>, ICompAbilityEffectOnJumpCompleted
    {
        protected override bool ExtraReady(Pawn p)
        {
            return Props.requiredHediff == null
                || p.health.hediffSet.HasHediff(Props.requiredHediff, false);
        }

        protected override void Relocate(Pawn p, IntVec3 cell, Map map, Pawn prey)
        {
            SVCast.Sound(Props.leapSound, p);
            IntVec3 origin = p.Position;
            p.jobs.EndCurrentJob(JobCondition.Succeeded, false, true);
            PawnFlyer flyer = PawnFlyer.MakeFlyer(Props.flyerDef, p, cell, null, Props.landSound,
                false, null, parent, prey);
            FleckMaker.ThrowDustPuff(origin.ToVector3Shifted(), map, 2f);
            GenSpawn.Spawn(flyer, cell, map, WipeMode.Vanish);
        }

        public void OnJumpCompleted(IntVec3 origin, LocalTargetInfo target)
        {
            Pawn p = parent.pawn;
            Pawn prey = target.Pawn;
            if (!p.Spawned || prey == null || !prey.Spawned || prey.Map != p.Map)
            {
                return;
            }
            p.meleeVerbs.TryMeleeAttack(prey, null, true);
        }
    }
}
