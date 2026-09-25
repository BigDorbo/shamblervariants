using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVTether : CompAbilityEffect_SVGated<CompProperties_SVAbilityTether>
    {
        public override bool CanCast
        {
            get { return !HasLiveTether(parent.pawn); }
        }

        private bool HasLiveTether(Pawn caster)
        {
            Map map = caster.MapHeld;
            if (map == null)
            {
                return false;
            }
            List<Thing> tethers = map.listerThings.ThingsOfDef(Props.tetherDef);
            for (int i = 0; i < tethers.Count; i++)
            {
                if (((Thing_SVTanglerKnot)tethers[i]).caster == caster)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            Pawn v = target.Pawn;
            return SVScan.Victim(v) && v.HostileTo(parent.pawn)
                && !v.health.hediffSet.HasHediff(Props.pinHediff, false);
        }

        private IntVec3 KnotCell(Pawn caster, Pawn v, Map map)
        {
            IntVec3 victimCell = v.Position;
            int cells = GenRadial.NumCellsInRadius(Props.knotPlacementRadius);
            IntVec3 best = victimCell;
            float bestDist = float.MaxValue;
            for (int i = 0; i < cells; i++)
            {
                IntVec3 cell = victimCell + GenRadial.RadialPattern[i];
                if (cell.InBounds(map) && cell.Standable(map) && SVScan.Closer(cell, caster.Position, ref bestDist))
                {
                    best = cell;
                }
            }
            return best;
        }

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn caster = parent.pawn;
            Pawn v = target.Pawn;
            Map map = v.Map;
            SVCast.Sound(Props.castSound, caster);
            v.health.AddHediff(Props.pinHediff, null, null, null);
            Thing_SVTanglerKnot tether = (Thing_SVTanglerKnot)ThingMaker.MakeThing(Props.tetherDef, null);
            tether.caster = caster;
            tether.victim = v;
            tether.pinHediff = Props.pinHediff;
            tether.maxLength = Props.maxLength;
            GenSpawn.Spawn(tether, KnotCell(caster, v, map), map, WipeMode.Vanish);
            tether.SetFaction(caster.Faction, null);
            SVCast.MetaIcon(Props.targetFleck, v.Position, map);
        }
    }
}
