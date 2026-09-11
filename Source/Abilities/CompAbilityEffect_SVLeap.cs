using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVLeap : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityLeap>, ICompAbilityEffectOnJumpCompleted
    {

        protected override bool ExtraReady(Pawn p)
        {
            return Props.requiredHediff == null
                || p.health.hediffSet.HasHediff(Props.requiredHediff, false);
        }

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
            Map map = p.Map;
            if (Props.leapSound != null)
            {
                Props.leapSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
            IntVec3 origin = p.Position;
            Pawn prey = SVScan.NearestHostile(p, Props.searchRadius);
            PawnFlyer flyer = PawnFlyer.MakeFlyer(Props.flyerDef, p, cell, null, Props.landSound,
                false, null, parent, prey);
            FleckMaker.ThrowDustPuff(origin.ToVector3Shifted(), map, 2f);
            GenSpawn.Spawn(flyer, cell, map, WipeMode.Vanish);
        }

        public void OnJumpCompleted(IntVec3 origin, LocalTargetInfo target)
        {
            Pawn p = parent.pawn;
            Pawn prey = target.Pawn;
            if (!p.Spawned || p.Dead
                || prey == null || prey.Dead || !prey.Spawned || prey.Map != p.Map)
            {
                return;
            }
            p.meleeVerbs.TryMeleeAttack(prey, null, true);
        }
    }
}
