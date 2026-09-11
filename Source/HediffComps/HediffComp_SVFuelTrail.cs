using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVFuelTrail : HediffComp_SV<HediffCompProperties_SVFuelTrail>
    {
        private bool engaged;


        public override void CompExposeData()
        {
            Scribe_Values.Look<bool>(ref engaged, "SV_fuelTrailEngaged", false, false);
        }

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            engaged = true;
        }

        private bool Engaged(Pawn p)
        {
            if (engaged)
            {
                return true;
            }
            if (p.mindState.enemyTarget != null)
            {
                engaged = true;
                return true;
            }
            if (p.mindState.lastEngageTargetTick > 0
                && Find.TickManager.TicksGame - p.mindState.lastEngageTargetTick < 2500)
            {
                engaged = true;
                return true;
            }
            return false;
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (!p.Spawned || p.Dead)
            {
                return;
            }
            if (Props.onlyWhenEngaged && !Engaged(p))
            {
                return;
            }
            bool moving = p.pather != null && p.pather.MovingNow;
            int interval = moving ? Props.moveIntervalTicks : Props.idleIntervalTicks;
            if (p.IsHashIntervalTick(interval, delta))
            {
                FilthMaker.TryMakeFilth(p.Position, p.Map, Props.filthDef, 1, FilthSourceFlags.None, true);
            }
        }
    }
}
