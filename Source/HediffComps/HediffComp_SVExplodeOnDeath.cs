using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVExplodeOnDeath : HediffComp_SV<HediffCompProperties_SVExplodeOnDeath>
    {
        private bool detonated;


        public override void CompExposeData()
        {
            Scribe_Values.Look<bool>(ref detonated, "SV_detonated", false, false);
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            if (detonated || !Props.detonateWhenBurning)
            {
                return;
            }
            Pawn p = Pawn;
            if (!p.Spawned || p.Dead)
            {
                return;
            }
            if (!p.IsHashIntervalTick(Props.burnCheckInterval, delta) || !p.IsBurning())
            {
                return;
            }
            DamageDef dam = Props.damageDef != null ? Props.damageDef : DamageDefOf.Flame;
            Detonate();
            if (!p.Dead)
            {
                p.Kill(new DamageInfo(dam, 99999f, 999f, -1f, p), null);
            }
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            Detonate();
        }

        private void Detonate()
        {
            if (detonated)
            {
                return;
            }
            detonated = true;
            Pawn p = Pawn;
            Map map = p.MapHeld;
            IntVec3 pos = p.PositionHeld;
            if (map == null)
            {
                return;
            }
            if (Props.radius <= 0f)
            {
                Gore(p, map, pos);
                return;
            }
            DamageDef dam = Props.damageDef != null ? Props.damageDef : DamageDefOf.Flame;
            GenExplosion.DoExplosion(pos, map, Props.radius, dam, p,
                damAmount: Props.damAmount,
                postExplosionSpawnThingDef: Props.spreadFilth,
                postExplosionSpawnChance: Props.spreadChance,
                postExplosionSpawnThingCount: Props.spreadCount,
                chanceToStartFire: Props.fireChance);
            Gore(p, map, pos);
        }

        private void Gore(Pawn p, Map map, IntVec3 pos)
        {
            if (!Props.gore)
            {
                return;
            }
            FleshbeastUtility.MeatSplatter(0, pos, map, FleshbeastUtility.MeatExplosionSize.Large);
            if (Props.goreFilthDef != null && Props.goreFilthCount > 0)
            {
                int cells = GenRadial.NumCellsInRadius(Props.goreFilthRadius);
                for (int i = 0; i < Props.goreFilthCount; i++)
                {
                    IntVec3 cell = pos + GenRadial.RadialPattern[Rand.Range(0, cells)];
                    if (cell.InBounds(map))
                    {
                        FilthMaker.TryMakeFilth(cell, map, Props.goreFilthDef, 1, FilthSourceFlags.None, true);
                    }
                }
            }
            if (!Props.destroyCorpse)
            {
                return;
            }
            MapComponent_SVCleanup.QueueDestroy(p.Corpse);
        }
    }
}
