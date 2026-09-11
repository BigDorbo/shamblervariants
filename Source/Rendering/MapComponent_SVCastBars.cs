using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    public class MapComponent_SVCastBars : MapComponent
    {
        private const int RefreshInterval = 10;
        private const float OffsetZ = -0.8f;

        private struct CasterEntry
        {
            public Pawn pawn;
            public Verb warmup;
            public HediffComp_SVPrimed primed;
        }

        private readonly List<CasterEntry> casters = new List<CasterEntry>();
        private readonly HashSet<Pawn> registry = new HashSet<Pawn>();
        private readonly List<Pawn> gone = new List<Pawn>();
        private readonly List<Pawn> stale = new List<Pawn>();
        private readonly Dictionary<Pawn, Effecter> bars = new Dictionary<Pawn, Effecter>();

        public MapComponent_SVCastBars(Map map) : base(map)
        {
        }

        public static void Register(Pawn p)
        {
            p.Map.GetComponent<MapComponent_SVCastBars>().registry.Add(p);
        }

        private static HediffComp_SVPrimed Primed(Pawn p)
        {
            if (!p.IsMutant)
            {
                return null;
            }
            List<Hediff> hediffs = p.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                HediffComp_SVPrimed comp = hediffs[i].TryGetComp<HediffComp_SVPrimed>();
                if (comp != null)
                {
                    return comp;
                }
            }
            return null;
        }

        private static Verb OurWarmup(Pawn p)
        {
            Stance_Warmup stance = p.stances.curStance as Stance_Warmup;
            if (stance == null || stance.verb == null)
            {
                return null;
            }
            Verb_CastAbility verb = stance.verb as Verb_CastAbility;
            if (verb == null || verb.Ability == null || verb.Ability.def == null)
            {
                return null;
            }
            if (!verb.Ability.def.showCastingProgressBar)
            {
                return null;
            }
            return p.IsColonistPlayerControlled ? null : stance.verb;
        }

        public override void MapComponentTick()
        {
            if (Find.TickManager.TicksGame % RefreshInterval != 0)
            {
                return;
            }
            casters.Clear();
            gone.Clear();
            foreach (Pawn c in registry)
            {
                if (!c.Spawned || c.Dead || c.Map != map)
                {
                    gone.Add(c);
                    continue;
                }
                Verb w = OurWarmup(c);
                HediffComp_SVPrimed pr = w == null ? Primed(c) : null;
                if (w != null || pr != null)
                {
                    casters.Add(new CasterEntry { pawn = c, warmup = w, primed = pr });
                }
            }
            for (int i = 0; i < gone.Count; i++)
            {
                registry.Remove(gone[i]);
            }
            stale.Clear();
            foreach (KeyValuePair<Pawn, Effecter> entry in bars)
            {
                bool live = false;
                for (int i = 0; i < casters.Count; i++)
                {
                    if (casters[i].pawn == entry.Key)
                    {
                        live = true;
                        break;
                    }
                }
                if (!live)
                {
                    stale.Add(entry.Key);
                }
            }
            for (int i = 0; i < stale.Count; i++)
            {
                Drop(stale[i]);
            }
        }

        public override void MapComponentUpdate()
        {
            for (int i = casters.Count - 1; i >= 0; i--)
            {
                Pawn p = casters[i].pawn;
                Verb verb = casters[i].warmup;
                if (verb != null)
                {
                    Stance_Warmup cur = p.stances != null ? p.stances.curStance as Stance_Warmup : null;
                    if (cur == null || cur.verb != verb)
                    {
                        verb = null;
                    }
                }
                HediffComp_SVPrimed primed = casters[i].primed;
                if ((verb == null && primed == null) || !p.Spawned || p.Dead || p.Map != map)
                {
                    Drop(p);
                    casters.RemoveAt(i);
                    continue;
                }
                Effecter bar;
                if (!bars.TryGetValue(p, out bar) || bar == null)
                {
                    bar = EffecterDefOf.ProgressBar.Spawn();
                    bars[p] = bar;
                    continue;
                }
                bar.EffectTick(p, TargetInfo.Invalid);
                SubEffecter_ProgressBar sub = (SubEffecter_ProgressBar)bar.children[0];
                if (sub.mote == null)
                {
                    continue;
                }
                sub.mote.progress = Mathf.Clamp01(verb != null ? verb.WarmupProgress : primed.Progress);
                sub.mote.offsetZ = OffsetZ;
                sub.mote.alwaysShow = true;
            }
        }

        private void Drop(Pawn p)
        {
            Effecter bar;
            if (bars.TryGetValue(p, out bar))
            {
                if (bar != null)
                {
                    bar.Cleanup();
                }
                bars.Remove(p);
            }
        }
    }
}
