using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVBlink : CompAbilityEffect_SVRelocate<CompProperties_SVAbilityBlink>
    {

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn p = parent.pawn;
            if (!p.Spawned || p.Dead)
            {
                return;
            }
            IntVec3 cell;
            Pawn mark = target.Pawn;
            if (mark == null || !SVCast.BehindCell(p, mark, 2.9f, out cell))
            {
                if (!SVCast.ResolveCell(p, target, dest, out cell))
                {
                    return;
                }
            }
            Map map = p.Map;
            SVCast.Teleport(p, cell, Props.originEffecter, Props.arriveEffecter);
            if (Props.arriveSound != null)
            {
                Props.arriveSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
            Pawn prey = SVScan.NearestHostile(p, Props.arriveScanRadius);
            if (prey == null)
            {
                return;
            }
            SVCast.Effect(Props.targetEffecter, prey.Position, map);
            if (Props.cloneOnArrive && !p.health.hediffSet.HasHediff(Props.echoHediff, false))
            {
                Hediff_SVGloamAnchor anchor = (Hediff_SVGloamAnchor)p.health.hediffSet.GetFirstHediffOfDef(Props.anchorHediff, false);
                if (anchor == null)
                {
                    anchor = (Hediff_SVGloamAnchor)p.health.AddHediff(Props.anchorHediff, null, null, null);
                }
                if (!anchor.EchoActive)
                {
                    anchor.echo = SpawnEcho(p, cell, map);
                }
            }
            if (Props.stunTicks > 0 && prey.stances != null)
            {
                prey.stances.stunner.StunFor(Props.stunTicks, p, true, true, false);
            }
        }

        private Pawn SpawnEcho(Pawn p, IntVec3 cell, Map map)
        {
            IntVec3 spot = IntVec3.Invalid;
            int cells = GenRadial.NumCellsInRadius(1.9f);
            for (int i = 1; i < cells; i++)
            {
                IntVec3 c = cell + GenRadial.RadialPattern[i];
                if (c.InBounds(map) && c.Standable(map))
                {
                    spot = c;
                    break;
                }
            }
            if (!spot.IsValid)
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
