using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVPrime : CompAbilityEffect_SVGated<CompProperties_SVAbilityPrime>
    {
        protected override bool ReadyFor(LocalTargetInfo target)
        {
            Pawn p = parent.pawn;
            return !p.health.hediffSet.HasHediff(Props.primedHediff, false)
                && SVScan.HostileNear(p, Props.requiredHostileRadius);
        }

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            parent.pawn.health.AddHediff(Props.primedHediff, null, null, null);
        }
    }
}
