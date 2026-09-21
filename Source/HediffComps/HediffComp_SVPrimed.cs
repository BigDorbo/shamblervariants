using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class HediffComp_SVPrimed : HediffComp_SV<HediffCompProperties_SVPrimed>
    {
        private Effecter effecter;


        public float Progress
        {
            get { return (float)parent.ageTicks / Props.durationTicks; }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            if (!p.Spawned)
            {
                return;
            }
            if (Props.startSound != null)
            {
                Props.startSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (p.Dead || !p.Spawned)
            {
                EndEffecter();
                return;
            }
            if (p.Downed || p.stances.stunner.Stunned)
            {
                Abort(p);
                return;
            }
            if (Props.effecter != null)
            {
                if (effecter == null)
                {
                    effecter = Props.effecter.Spawn(p, p.Map, 1f);
                }
                effecter.EffectTick(p, p);
            }
            if (parent.ageTicks >= Props.durationTicks)
            {
                Detonate(p);
            }
        }

        public override void CompPostPostRemoved()
        {
            EndEffecter();
        }

        private void Abort(Pawn p)
        {
            EndEffecter();
            p.health.RemoveHediff(parent);
        }

        private void Detonate(Pawn p)
        {
            EndEffecter();
            p.health.RemoveHediff(parent);
            SVCast.Kill(p, Props.damageDef);
        }

        private void EndEffecter()
        {
            if (effecter != null)
            {
                effecter.Cleanup();
                effecter = null;
            }
        }
    }
}
