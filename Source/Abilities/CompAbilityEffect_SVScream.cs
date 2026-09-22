using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVScream : CompAbilityEffect_SVGated<CompProperties_SVAbilityScream>
    {

        private bool Scareable(Pawn other)
        {
            Pawn caster = parent.pawn;
            return SVScan.ValidVariantTarget(other) && other != caster
                && !other.Dead && !other.Downed
                && !other.IsMutant && !other.IsShambler
                && other.RaceProps.Humanlike
                && other.mindState != null && other.mindState.mentalStateHandler != null
                && !other.InMentalState
                && other.HostileTo(caster)
                && (Props.applyHediff == null
                    || !other.health.hediffSet.HasHediff(Props.applyHediff, false));
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            return Scareable(target.Pawn);
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn p = parent.pawn;
            Pawn other = target.Pawn;
            if (!p.Spawned || !Scareable(other))
            {
                return;
            }
            Map map = p.Map;
            if (Props.screamSound != null)
            {
                Props.screamSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
            if (Props.blastEffecter != null && map != null)
            {
                Effecter blast = Props.blastEffecter.Spawn();
                blast.Trigger(new TargetInfo(p), new TargetInfo(other), -1);
                blast.Cleanup();
            }
            if (Props.applyHediff != null)
            {
                SVHeal.Mark(other, Props.applyHediff, Props.hediffDurationTicks);
            }
            if (Props.mentalState != null)
            {
                other.mindState.mentalStateHandler.TryStartMentalState(
                    Props.mentalState, null, true, true, false, p, false, false, false);
            }
        }
    }
}
