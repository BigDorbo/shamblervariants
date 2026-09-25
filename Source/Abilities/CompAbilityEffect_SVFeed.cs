using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVFeed : CompAbilityEffect_SVGated<CompProperties_SVAbilityFeed>
    {
        private bool wasWarming;

        public override void CompTick()
        {
            base.CompTick();
            bool warming = parent.verb.WarmingUp;
            if (warming == wasWarming)
            {
                return;
            }
            wasWarming = warming;
            parent.pawn.Drawer.renderer.SetAllGraphicsDirty();
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            Pawn v = target.Pawn;
            return SVScan.Victim(v) && v.RaceProps.IsFlesh && SVScan.ValidVariantTarget(v) && !v.IsBurning();
        }

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn p = parent.pawn;
            Pawn v = target.Pawn;
            Map map = v.Map;
            IntVec3 at = v.Position;
            HealthUtility.AdjustSeverity(v, HediffDefOf.BloodLoss, Props.bloodLoss);
            Props.Mark(v);
            if (Props.stunTicks > 0 && !v.Downed)
            {
                v.stances.stunner.StunFor(Props.stunTicks, null, false, true, false);
            }
            SVHeal.HealWounds(p, Props.healAmount, Props.maxWounds);
            FleshbeastUtility.MeatSplatter(0, at, map, FleshbeastUtility.MeatExplosionSize.Normal);
            SVCast.Splatter(Props.filthDef, at, map, Props.filthCount, Props.filthRadius);
            SVCast.Sound(Props.biteSound, p);
        }
    }
}
