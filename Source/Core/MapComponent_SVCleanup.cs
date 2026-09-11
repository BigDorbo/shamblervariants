using System.Collections.Generic;
using Verse;

namespace ShamblerVariants
{
    public class MapComponent_SVCleanup : MapComponent
    {
        private readonly List<Thing> pending = new List<Thing>();

        public MapComponent_SVCleanup(Map map) : base(map)
        {
        }

        public static void QueueDestroy(Thing thing)
        {
            if (thing == null || thing.Destroyed || thing.Map == null)
            {
                return;
            }
            MapComponent_SVCleanup comp = thing.Map.GetComponent<MapComponent_SVCleanup>();
            if (comp != null && !comp.pending.Contains(thing))
            {
                comp.pending.Add(thing);
            }
        }

        public override void MapComponentTick()
        {
            if (pending.Count == 0)
            {
                return;
            }
            for (int i = pending.Count - 1; i >= 0; i--)
            {
                Thing thing = pending[i];
                if (thing != null && !thing.Destroyed && thing.Spawned)
                {
                    thing.Destroy(DestroyMode.Vanish);
                }
            }
            pending.Clear();
        }
    }
}
