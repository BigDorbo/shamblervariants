using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVBlink : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityBlink>
    {
        protected override bool PickCell(Pawn p, LocalTargetInfo target, LocalTargetInfo dest, out IntVec3 cell)
        {
            Pawn mark = target.Pawn;
            if (mark != null && mark != p && SVCast.BehindCell(p, mark, Props.arriveScanRadius, out cell))
            {
                return true;
            }
            return base.PickCell(p, target, dest, out cell);
        }

        protected override void Relocate(Pawn p, IntVec3 cell, Map map, Pawn prey)
        {
            SVCast.Teleport(p, cell, Props.originEffecter, Props.arriveEffecter);
            SVCast.Sound(Props.arriveSound, p);
            Pawn near = SVScan.NearestHostile(p, Props.arriveScanRadius);
            if (near == null)
            {
                return;
            }
            SVCast.Effect(Props.targetEffecter, near.Position, map);
            if (Props.cloneOnArrive && !p.health.hediffSet.HasHediff(Props.echoHediff, false))
            {
                Hediff_SVGloamAnchor anchor = (Hediff_SVGloamAnchor)p.health.GetOrAddHediff(Props.anchorHediff, null, null, null);
                if (!anchor.EchoActive)
                {
                    anchor.echo = SpawnEcho(p, cell, map);
                }
            }
            if (Props.stunTicks > 0)
            {
                near.stances.stunner.StunFor(Props.stunTicks, p, true, true, false);
            }
        }

        private Pawn SpawnEcho(Pawn p, IntVec3 cell, Map map)
        {
            IntVec3 spot;
            if (!SVCast.NearStandable(cell, map, 1, cell, out spot))
            {
                return null;
            }
            Pawn echo = Find.PawnDuplicator.Duplicate(p);
            echo.health.AddHediff(Props.echoHediff, null, null, null);
            GenSpawn.Spawn(echo, spot, map, WipeMode.Vanish);
            int guard = 0;
            while (echo.health.summaryHealth.SummaryHealthPercent > Props.cloneHealthFraction
                && !echo.Dead && guard < 60)
            {
                echo.TakeDamage(new DamageInfo(DamageDefOf.Cut, 4f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, false, QualityCategory.Normal, false, false));
                guard++;
            }
            SVCast.Effect(Props.arriveEffecter, spot, map);
            return echo;
        }
    }
}
