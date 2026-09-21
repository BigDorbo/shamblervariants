using RimWorld;
using Verse;

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
