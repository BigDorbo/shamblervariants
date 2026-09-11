using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVTether : CompAbilityEffect_SV<CompProperties_SVAbilityTether>
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

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!base.Valid(target, throwMessages))
            {
                return false;
            }
            Pawn v = target.Pawn;
            if (v == null || v.Dead || v.IsMutant || !v.HostileTo(parent.pawn))
            {
                return false;
            }
            if (Props.pinHediff != null && v.health.hediffSet.HasHediff(Props.pinHediff, false))
            {
                return false;
            }
            return !HasLiveTether(parent.pawn);
        }

        private IntVec3 KnotCell(Pawn caster, Pawn v, Map map)
        {
            IntVec3 victimCell = v.Position;
            float reach = Props.knotPlacementRadius;
            int cells = GenRadial.NumCellsInRadius(reach);
            IntVec3 best = victimCell;
            float bestDist = -1f;
            for (int i = 0; i < cells; i++)
            {
                IntVec3 cell = victimCell + GenRadial.RadialPattern[i];
                if (!cell.InBounds(map) || !cell.Standable(map))
                {
                    continue;
                }
                float d = (cell - caster.Position).LengthHorizontalSquared;
                if (bestDist < 0f || d < bestDist)
                {
                    bestDist = d;
                    best = cell;
                }
            }
            return best;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn caster = parent.pawn;
            Pawn v = target.Pawn;
            Map map = caster.MapHeld;
            if (map == null || v == null || v.Dead || !v.Spawned)
            {
                return;
            }
            if (Props.castSound != null)
            {
                Props.castSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(caster), MaintenanceType.None));
            }
            if (!v.health.hediffSet.HasHediff(Props.pinHediff, false))
            {
                v.health.AddHediff(Props.pinHediff, null, null, null);
            }
            IntVec3 mid = KnotCell(caster, v, map);
            Thing_SVTanglerKnot tether = (Thing_SVTanglerKnot)ThingMaker.MakeThing(Props.tetherDef, null);
            tether.caster = caster;
            tether.victim = v;
            tether.pinHediff = Props.pinHediff;
            tether.maxLength = Props.maxLength;
            GenSpawn.Spawn(tether, mid, map, WipeMode.Vanish);
            tether.SetFaction(caster.Faction, null);
            if (Props.targetFleck != null)
            {
                FleckMaker.ThrowMetaIcon(v.Position, map, Props.targetFleck, 0.42f);
            }
        }
    }
}
