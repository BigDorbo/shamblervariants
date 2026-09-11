using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVPrime : CompAbilityEffect_SVGated<CompProperties_SVAbilityPrime>
    {

        private bool Ready()
        {
            Pawn p = parent.pawn;
            if (!SVCast.CasterReady(p))
            {
                return false;
            }
            if (p.health.hediffSet.HasHediff(Props.primedHediff, false))
            {
                return false;
            }
            return SVScan.HostileNear(p, Props.requiredHostileRadius);
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            return Ready();
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn p = parent.pawn;
            if (!Ready())
            {
                return;
            }
            p.health.AddHediff(Props.primedHediff, null, null, null);
        }
    }
}
