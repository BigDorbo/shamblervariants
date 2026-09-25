using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVScream : CompAbilityEffect_SVGated<CompProperties_SVAbilityScream>
    {
        protected override bool ReadyFor(LocalTargetInfo target)
        {
            Pawn caster = parent.pawn;
            Pawn other = target.Pawn;
            return SVScan.Victim(other) && other != caster && !other.Downed
                && other.RaceProps.Humanlike && !other.InMentalState
                && other.HostileTo(caster)
                && (Props.applyHediff == null
                    || !other.health.hediffSet.HasHediff(Props.applyHediff, false));
        }

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn p = parent.pawn;
            Pawn other = target.Pawn;
            SVCast.Sound(Props.screamSound, p);
            if (Props.blastEffecter != null)
            {
                Props.blastEffecter.Spawn(new TargetInfo(p), new TargetInfo(other), 1f).Cleanup();
            }
            Props.Mark(other);
            if (Props.mentalState != null)
            {
                other.mindState.mentalStateHandler.TryStartMentalState(
                    Props.mentalState, null, true, true, false, p, false, false, false);
            }
        }
    }
}
