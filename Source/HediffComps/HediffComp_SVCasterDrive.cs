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
            if (!p.Spawned || p.Dead || p.Downed)
            {
                return;
            }
            if (!p.IsHashIntervalTick(Props.checkInterval, delta))
            {
                return;
            }
            MapComponent_SVCastBars.Register(p);
            if (p.abilities == null || (p.CurJob != null && p.CurJob.ability != null))
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
            if (ab == null)
            {
                p.abilities.GainAbility(cast.ability);
                ab = p.abilities.GetAbility(cast.ability, false);
            }
            if (ab == null || !ab.CanCast)
            {
                return false;
            }
            if (cast.targetCorpses || cast.targetGraves)
            {
                Thing thing = FindDead(p, cast);
                if (thing == null || !AbilityUsableOn(ab, thing))
                {
                    return false;
                }
                Job deadJob = ab.GetJob(thing, thing);
                return SVCast.Launch(p, deadJob);
            }
            Pawn target = FindTarget(p, cast);
            if (target == null || !AbilityUsableOn(ab, target))
            {
                return false;
            }
            Job job;
            if (cast.skipToTarget)
            {
                Map map = p.Map;
                IntVec3 dest;
                if (map == null || !CellFinder.TryFindRandomCellNear(target.Position, map, cast.arriveRadius,
                    delegate(IntVec3 c) { return c.Standable(map) && c != p.Position; }, out dest, -1))
                {
                    return false;
                }
                job = ab.GetJob(p, dest);
            }
            else
            {
                job = ab.GetJob(target, target);
            }
            return SVCast.Launch(p, job);
        }

        private static bool AbilityUsableOn(Ability ab, LocalTargetInfo target)
        {
            List<CompAbilityEffect> comps = ab.EffectComps;
            if (comps == null)
            {
                return true;
            }
            for (int i = 0; i < comps.Count; i++)
            {
                if (!comps[i].AICanTargetNow(target))
                {
                    return false;
                }
            }
            return true;
        }

        private static Thing FindDead(Pawn p, SVAbilityCast cast)
        {
            Map map = p.Map;
            if (map == null)
            {
                return null;
            }
            Thing best = null;
            float bestDist = cast.searchRadius * cast.searchRadius;
            if (cast.targetCorpses)
            {
                List<Thing> corpses = map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
                for (int i = 0; i < corpses.Count; i++)
                {
                    Corpse corpse = (Corpse)corpses[i];
                    if (corpse.Destroyed || !corpse.Spawned
                        || !MutantUtility.CanResurrectAsShambler(corpse, true))
                    {
                        continue;
                    }
                    float d = (corpse.Position - p.Position).LengthHorizontalSquared;
                    if (d < bestDist)
                    {
                        bestDist = d;
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
                    if (casket.Destroyed || !casket.HasAnyContents)
                    {
                        continue;
                    }
                    Corpse held = casket.ContainedThing as Corpse;
                    if (held == null || !MutantUtility.CanResurrectAsShambler(held, true))
                    {
                        continue;
                    }
                    float d = (casket.Position - p.Position).LengthHorizontalSquared;
                    if (d < bestDist)
                    {
                        bestDist = d;
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
                if (cast.requireEnemyTarget
                    && p.mindState.enemyTarget == null)
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
                    Consider(p, cast, kin[i], ref best, ref bestDist);
                }
            }
            if (cast.targetHostiles)
            {
                foreach (IAttackTarget target in p.Map.attackTargetsCache.TargetsHostileToFaction(p.Faction))
                {
                    Consider(p, cast, target.Thing as Pawn, ref best, ref bestDist);
                }
            }
            return best;
        }

        private static void Consider(Pawn p, SVAbilityCast cast, Pawn other, ref Pawn best, ref float bestDist)
        {
            if (other == null || other.Dead)
            {
                return;
            }
            if (other == p && !cast.targetWoundedShamblers)
            {
                return;
            }
            bool valid = (cast.targetWoundedShamblers && other.IsShambler && SVCast.SameSide(other, p) && SVHeal.HasInjury(other))
                || (cast.targetHostiles && !other.Downed && SVScan.ValidVariantTarget(other) && other.HostileTo(p));
            if (!valid)
            {
                return;
            }
            if (cast.skipIfTargetHas != null && other.health.hediffSet.HasHediff(cast.skipIfTargetHas, false))
            {
                return;
            }
            float d = (other.Position - p.Position).LengthHorizontalSquared;
            if (d <= bestDist)
            {
                bestDist = d;
                best = other;
            }
        }
    }
}
