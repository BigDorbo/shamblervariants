using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    public class HediffComp_SVCasterDrive : HediffComp_SV<HediffCompProperties_SVCasterDrive>
    {
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (!Ours(p) || p.Downed || !p.IsHashIntervalTick(Props.checkInterval, delta))
            {
                return;
            }
            MapComponent_SVCastBars.Register(p);
            if (p.CurJob != null && p.CurJob.ability != null)
            {
                return;
            }
            for (int i = 0; i < Props.casts.Count; i++)
            {
                if (TryCast(p, Props.casts[i]))
                {
                    return;
                }
            }
        }

        private static bool TryCast(Pawn p, SVAbilityCast cast)
        {
            Ability ab = p.abilities.GetAbility(cast.ability, false);
            if (!ab.CanCast)
            {
                return false;
            }
            if (cast.targetCorpses || cast.targetGraves)
            {
                Thing thing = FindDead(p, cast);
                if (thing == null || !ab.AICanTargetNow(thing))
                {
                    return false;
                }
                SVCast.Launch(p, ab.GetJob(thing, thing));
                return true;
            }
            Pawn target = FindTarget(p, cast);
            if (target == null || !ab.AICanTargetNow(target))
            {
                return false;
            }
            Job job;
            if (cast.skipToTarget)
            {
                IntVec3 dest;
                if (!SVCast.NearStandable(target.Position, p.Map, cast.arriveRadius, IntVec3.Invalid, out dest))
                {
                    return false;
                }
                job = ab.GetJob(p, dest);
            }
            else
            {
                job = ab.GetJob(target, target);
            }
            SVCast.Launch(p, job);
            return true;
        }

        private static Thing FindDead(Pawn p, SVAbilityCast cast)
        {
            Map map = p.Map;
            Thing best = null;
            float bestDist = cast.searchRadius * cast.searchRadius;
            if (cast.targetCorpses)
            {
                List<Thing> corpses = map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
                for (int i = 0; i < corpses.Count; i++)
                {
                    Corpse corpse = (Corpse)corpses[i];
                    if (MutantUtility.CanResurrectAsShambler(corpse, true) && SVScan.Closer(corpse.Position, p.Position, ref bestDist))
                    {
                        best = corpse;
                    }
                }
            }
            if (cast.targetGraves)
            {
                List<Thing> graves = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave);
                for (int i = 0; i < graves.Count; i++)
                {
                    Building_Casket casket = (Building_Casket)graves[i];
                    if (casket.HasAnyContents && MutantUtility.CanResurrectAsShambler(casket.ContainedThing as Corpse, true)
                        && SVScan.Closer(casket.Position, p.Position, ref bestDist))
                    {
                        best = casket;
                    }
                }
            }
            return best;
        }

        private static Pawn FindTarget(Pawn p, SVAbilityCast cast)
        {
            if (cast.selfCast)
            {
                if (cast.requireEnemyTarget && p.mindState.enemyTarget == null)
                {
                    return null;
                }
                if (cast.requireHostileNear && !SVScan.HostileNear(p, cast.hostileNearRadius))
                {
                    return null;
                }
                return p;
            }
            if (p.Faction == null)
            {
                return null;
            }
            Pawn best = null;
            float bestDist = cast.searchRadius * cast.searchRadius;
            if (cast.targetWoundedShamblers)
            {
                List<Pawn> kin = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction);
                for (int i = 0; i < kin.Count; i++)
                {
                    Pawn other = kin[i];
                    if (other.IsShambler && other.health.hediffSet.HasHediff<Hediff_Injury>(false)
                        && (cast.skipIfTargetHas == null || !other.health.hediffSet.HasHediff(cast.skipIfTargetHas, false))
                        && SVScan.Closer(other.Position, p.Position, ref bestDist))
                    {
                        best = other;
                    }
                }
            }
            if (cast.targetHostiles)
            {
                Pawn hostile = SVScan.NearestHostile(p, cast.searchRadius, cast.skipIfTargetHas);
                if (hostile != null && SVScan.Closer(hostile.Position, p.Position, ref bestDist))
                {
                    best = hostile;
                }
            }
            return best;
        }
    }
}
