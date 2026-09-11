using RimWorld;
using Verse;
using Verse.AI;

namespace ShamblerVariants
{
    [DefOf]
    public static class SVDefOf
    {

        public static AbilityDef SV_LatcherChomp;


        static SVDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SVDefOf));
        }
    }
}
