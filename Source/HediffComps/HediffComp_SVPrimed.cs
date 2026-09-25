using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVPrimed : HediffComp_SV<HediffCompProperties_SVPrimed>
    {
        public float Progress
        {
            get { return (float)parent.ageTicks / Props.durationTicks; }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            if (p.Spawned)
            {
                SVCast.Sound(Props.startSound, p);
            }
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (!p.Spawned)
            {
                return;
            }
            if (p.Downed || p.stances.stunner.Stunned)
            {
                p.health.RemoveHediff(parent);
                return;
            }
            if (parent.ageTicks >= Props.durationTicks)
            {
                p.health.RemoveHediff(parent);
                SVCast.Kill(p, Props.damageDef);
            }
        }
    }
}
