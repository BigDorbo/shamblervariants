using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVExplodeOnDeath : HediffComp_SV<HediffCompProperties_SVExplodeOnDeath>
    {
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (!Props.detonateWhenBurning || !p.Spawned
                || !p.IsHashIntervalTick(Props.burnCheckInterval, delta) || !p.IsBurning())
            {
                return;
            }
            SVCast.Kill(p, Props.damageDef);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            Pawn p = Pawn;
            Map map = p.MapHeld;
            if (map == null)
            {
                return;
            }
            IntVec3 pos = p.PositionHeld;
            if (Props.radius > 0f)
            {
                GenExplosion.DoExplosion(pos, map, Props.radius, Props.damageDef, p,
                    damAmount: Props.damAmount,
                    postExplosionSpawnThingDef: Props.spreadFilth,
                    postExplosionSpawnChance: Props.spreadChance,
                    postExplosionSpawnThingCount: Props.spreadCount,
                    chanceToStartFire: Props.fireChance);
            }
            if (!Props.gore)
            {
                return;
            }
            FleshbeastUtility.MeatSplatter(0, pos, map, FleshbeastUtility.MeatExplosionSize.Large);
            SVCast.Splatter(Props.goreFilthDef, pos, map, Props.goreFilthCount, Props.goreFilthRadius);
            if (Props.destroyCorpse)
            {
                MapComponent_SVCleanup.QueueDestroy(p.Corpse);
            }
        }
    }
}
