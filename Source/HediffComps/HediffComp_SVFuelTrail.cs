using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVFuelTrail : HediffComp_SV<HediffCompProperties_SVFuelTrail>
    {
        public const int EngageMemoryTicks = 2500;

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
            if (!engaged)
            {
                engaged = p.mindState.enemyTarget != null
                    || (p.mindState.lastEngageTargetTick > 0
                        && Find.TickManager.TicksGame - p.mindState.lastEngageTargetTick < EngageMemoryTicks);
            }
            return engaged;
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (!Ours(p) || (Props.onlyWhenEngaged && !Engaged(p)))
            {
                return;
            }
            int interval = p.pather.MovingNow ? Props.moveIntervalTicks : Props.idleIntervalTicks;
            if (p.IsHashIntervalTick(interval, delta))
            {
                FilthMaker.TryMakeFilth(p.Position, p.Map, Props.filthDef, 1, FilthSourceFlags.None, true);
            }
        }
    }
}
