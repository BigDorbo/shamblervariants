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
            if (thing == null || thing.Destroyed || thing.MapHeld == null)
            {
                return;
            }
            List<Thing> pending = thing.MapHeld.GetComponent<MapComponent_SVCleanup>().pending;
            if (!pending.Contains(thing))
            {
                pending.Add(thing);
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
                if (!pending[i].Destroyed)
                {
                    pending[i].Destroy(DestroyMode.Vanish);
                }
            }
            pending.Clear();
        }
    }
}
