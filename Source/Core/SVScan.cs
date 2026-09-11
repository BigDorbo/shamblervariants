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


        public static bool HostileNear(Pawn p, float radius)
        {
            return NearestHostile(p, radius) != null;
        }

        public static Pawn NearestHostile(Pawn p, float radius)
        {
            Map map = p.Map;
            if (map == null)
            {
                return null;
            }
            if (p.Faction == null)
            {
                return null;
            }
            Pawn best = null;
            float bestDist = radius * radius;
            foreach (IAttackTarget target in map.attackTargetsCache.TargetsHostileToFaction(p.Faction))
            {
                Pawn other = target.Thing as Pawn;
                if (other == null || other == p || other.Dead || other.Downed
                    || !ValidVariantTarget(other) || !other.HostileTo(p))
                {
                    continue;
                }
                float d = (other.Position - p.Position).LengthHorizontalSquared;
                if (d < bestDist)
                {
                    bestDist = d;
                    best = other;
                }
            }
            return best;
        }

        public static bool WithinSquared(IntVec3 a, IntVec3 b, float radius)
        {
            return (a - b).LengthHorizontalSquared <= radius * radius;
        }
    }
}
