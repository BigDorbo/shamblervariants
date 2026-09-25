using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace ShamblerVariants
{
    public static class SVCast
    {
        public const int EffectTicks = 60;

        public static void Effect(EffecterDef def, IntVec3 cell, Map map)
        {
            if (def != null)
            {
                map.effecterMaintainer.AddEffecterToMaintain(def.Spawn(cell, map, 1f), cell, EffectTicks);
            }
        }

        public static void Sound(SoundDef def, Thing at)
        {
            if (def != null)
            {
                def.PlayOneShot(SoundInfo.InMap(new TargetInfo(at), MaintenanceType.None));
            }
        }

        public static void MetaIcon(FleckDef def, IntVec3 cell, Map map)
        {
            if (def != null)
            {
                FleckMaker.ThrowMetaIcon(cell, map, def);
            }
        }

        public static void Splatter(ThingDef filth, IntVec3 at, Map map, int count, float radius)
        {
            if (filth == null || count <= 0)
            {
                return;
            }
            int cells = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < count; i++)
            {
                IntVec3 cell = at + GenRadial.RadialPattern[Rand.Range(0, cells)];
                if (cell.InBounds(map) && GenSight.LineOfSight(at, cell, map, false, null, 0, 0))
                {
                    FilthMaker.TryMakeFilth(cell, map, filth, 1, FilthSourceFlags.None, true);
                }
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

        public static void Launch(Pawn p, Job job)
        {
            p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
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
            return p.Spawned && !p.Downed;
        }

        public static Verb_CastAbility WarmingUp(Pawn p)
        {
            Stance_Warmup stance = p.stances.curStance as Stance_Warmup;
            if (stance == null)
            {
                return null;
            }
            Verb_CastAbility verb = stance.verb as Verb_CastAbility;
            return verb != null && verb.Ability != null ? verb : null;
        }

        public static void Kill(Pawn p, DamageDef dam)
        {
            p.Kill(new DamageInfo(dam, 99999f, 999f, -1f, p), null);
        }

        public static bool ResolveCell(Pawn p, LocalTargetInfo target, LocalTargetInfo dest, out IntVec3 cell)
        {
            cell = dest.IsValid ? dest.Cell : target.Cell;
            Map map = p.Map;
            if (!cell.InBounds(map))
            {
                return false;
            }
            return cell.Standable(map) || NearStandable(cell, map, 1, IntVec3.Invalid, out cell);
        }

        public static bool NearStandable(IntVec3 cell, Map map, int radius, IntVec3 exclude, out IntVec3 result)
        {
            return CellFinder.TryFindRandomCellNear(cell, map, radius,
                delegate(IntVec3 c) { return c != exclude && c.Standable(map); }, out result, -1);
        }
    }
}
