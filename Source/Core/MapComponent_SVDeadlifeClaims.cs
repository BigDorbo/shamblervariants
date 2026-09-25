using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class SVDeadlifeClaim : IExposable
    {
        public IntVec3 cell;
        public Faction faction;
        public float radius;
        public int tick;

        public void ExposeData()
        {
            Scribe_Values.Look<IntVec3>(ref cell, "cell", default(IntVec3), false);
            Scribe_References.Look<Faction>(ref faction, "faction", false);
            Scribe_Values.Look<float>(ref radius, "radius", 0f, false);
            Scribe_Values.Look<int>(ref tick, "tick", 0, false);
        }
    }

    public class MapComponent_SVDeadlifeClaims : MapComponent
    {
        public const int MarkTicks = 12500;

        private List<SVDeadlifeClaim> claims = new List<SVDeadlifeClaim>();

        public MapComponent_SVDeadlifeClaims(Map map) : base(map)
        {
        }

        public static void Add(Map map, IntVec3 cell, Faction faction, float radius)
        {
            MapComponent_SVDeadlifeClaims comp = map.GetComponent<MapComponent_SVDeadlifeClaims>();
            comp.Prune();
            comp.claims.Add(new SVDeadlifeClaim { cell = cell, faction = faction, radius = radius, tick = GenTicks.TicksGame });
        }

        public Faction OwnerAt(IntVec3 cell)
        {
            Prune();
            for (int i = claims.Count - 1; i >= 0; i--)
            {
                SVDeadlifeClaim claim = claims[i];
                if (cell.InHorDistOf(claim.cell, claim.radius))
                {
                    return claim.faction;
                }
            }
            return null;
        }

        private void Prune()
        {
            int now = GenTicks.TicksGame;
            for (int i = claims.Count - 1; i >= 0; i--)
            {
                if (now - claims[i].tick >= MarkTicks || claims[i].faction == null)
                {
                    claims.RemoveAt(i);
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look<SVDeadlifeClaim>(ref claims, "SV_deadlifeClaims", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && claims == null)
            {
                claims = new List<SVDeadlifeClaim>();
            }
        }
    }
}
