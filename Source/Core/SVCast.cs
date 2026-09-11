using RimWorld;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    public static class SVCast
    {
        public static void Effect(EffecterDef def, IntVec3 cell, Map map)
        {
            if (def != null)
            {
                def.Spawn(cell, map, 1f).Cleanup();
            }
        }

        public static void Teleport(Pawn p, IntVec3 cell, EffecterDef origin, EffecterDef arrive)
        {
            Map map = p.Map;
            Effect(origin, p.Position, map);
            p.Position = cell;
            p.Notify_Teleported(true, true);
            Effect(arrive, cell, map);
        }

        public static bool Launch(Pawn p, Job job, int expiry = -1)
        {
            if (job == null)
            {
                return false;
            }
            if (expiry > 0)
            {
                job.expiryInterval = expiry;
            }
            p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
            return true;
        }

        public static bool SameSide(Pawn a, Pawn b)
        {
            return a.Faction == b.Faction;
        }

        public static bool BehindCell(Pawn caster, Pawn prey, float radius, out IntVec3 cell)
        {
            cell = IntVec3.Invalid;
            Map map = prey.Map;
            if (map == null)
            {
                return false;
            }
            UnityEngine.Vector3 approach = (prey.Position - caster.Position).ToVector3();
            approach.Normalize();
            float best = 0f;
            int cells = GenRadial.NumCellsInRadius(radius);
            for (int i = 1; i < cells; i++)
            {
                IntVec3 c = prey.Position + GenRadial.RadialPattern[i];
                if (!c.InBounds(map) || !c.Standable(map))
                {
                    continue;
                }
                UnityEngine.Vector3 offset = (c - prey.Position).ToVector3();
                offset.Normalize();
                float score = UnityEngine.Vector3.Dot(offset, approach);
                if (score > best)
                {
                    best = score;
                    cell = c;
                }
            }
            return cell.IsValid;
        }

        public static bool CasterReady(Pawn p)
        {
            return p != null && p.Spawned && !p.Dead && !p.Downed;
        }

        public static bool ResolveCell(Pawn p, LocalTargetInfo target, LocalTargetInfo dest, out IntVec3 cell)
        {
            cell = dest.IsValid ? dest.Cell : target.Cell;
            Map map = p.Map;
            return map != null && cell.InBounds(map) && cell.Standable(map);
        }
    }
}
