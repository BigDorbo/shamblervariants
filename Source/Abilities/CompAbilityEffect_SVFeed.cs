using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVFeed : CompAbilityEffect_SV<CompProperties_SVAbilityFeed>
    {

        private bool wasWarming;

        public override void CompTick()
        {
            base.CompTick();
            Pawn p = parent.pawn;
            bool warming = parent.verb != null && parent.verb.WarmupStance != null;
            if (warming == wasWarming)
            {
                return;
            }
            wasWarming = warming;
            p.Drawer.renderer.SetAllGraphicsDirty();
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn v = target.Pawn;
            return v != null && !v.Dead && v.Spawned && !v.IsMutant
                && v.RaceProps.IsFlesh && SVScan.ValidVariantTarget(v)
                && !v.IsBurning() && base.Valid(target, throwMessages);
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn p = parent.pawn;
            Pawn v = target.Pawn;
            if (v == null || v.Dead || !v.Spawned)
            {
                return;
            }
            Map map = v.Map;
            IntVec3 at = v.Position;

            Hediff loss = v.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
            if (loss == null)
            {
                loss = v.health.AddHediff(HediffDefOf.BloodLoss, null, null, null);
                loss.Severity = Props.bloodLoss;
            }
            else
            {
                loss.Severity = loss.Severity + Props.bloodLoss;
            }
            if (Props.markHediff != null)
            {
                SVHeal.Mark(v, Props.markHediff, 0);
            }
            if (Props.stunTicks > 0 && !v.Downed && v.stances != null && v.stances.stunner != null)
            {
                v.stances.stunner.StunFor(Props.stunTicks, null, false, true, false);
            }

            SVHeal.HealWounds(p, Props.healAmount, Props.maxWounds);
            if (map != null)
            {
                FleshbeastUtility.MeatSplatter(0, at, map, FleshbeastUtility.MeatExplosionSize.Normal);
                if (Props.filthDef != null)
                {
                    int cells = GenRadial.NumCellsInRadius(Props.filthRadius);
                    for (int i = 0; i < Props.filthCount; i++)
                    {
                        IntVec3 cell = at + GenRadial.RadialPattern[Rand.Range(0, cells)];
                        if (cell.InBounds(map))
                        {
                            FilthMaker.TryMakeFilth(cell, map, Props.filthDef, 1, FilthSourceFlags.None, true);
                        }
                    }
                }
            }
            if (Props.biteSound != null)
            {
                Props.biteSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
        }
    }
}
