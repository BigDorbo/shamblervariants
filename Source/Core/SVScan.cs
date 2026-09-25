using RimWorld;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    public static class SVScan
    {
        public static bool ValidVariantTarget(Pawn other)
        {
            return other != null && !other.RaceProps.Animal;
        }

        public static bool Victim(Pawn v)
        {
            return v != null && v.Spawned && !v.IsMutant;
        }

        public static bool HostileNear(Pawn p, float radius)
        {
            return NearestHostile(p, radius) != null;
        }

        public static bool Closer(IntVec3 a, IntVec3 b, ref float bestDistSquared)
        {
            float d = (a - b).LengthHorizontalSquared;
            if (d < bestDistSquared)
            {
                bestDistSquared = d;
                return true;
            }
            return false;
        }

        public static Pawn NearestHostile(Pawn p, float radius, HediffDef skip = null)
        {
            Map map = p.Map;
            if (map == null || p.Faction == null)
            {
                return null;
            }
            Pawn best = null;
            float bestDist = radius * radius;
            foreach (IAttackTarget target in map.attackTargetsCache.TargetsHostileToFaction(p.Faction))
            {
                Pawn other = target.Thing as Pawn;
                if (other == null || other == p || other.Downed
                    || !ValidVariantTarget(other) || !other.HostileTo(p)
                    || (skip != null && other.health.hediffSet.HasHediff(skip, false)))
                {
                    continue;
                }
                if (Closer(other.Position, p.Position, ref bestDist))
                {
                    best = other;
                }
            }
            return best;
        }
    }
}
