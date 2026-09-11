using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [StaticConstructorOnStartup]
    public static class SVHarmonyInit
    {
        static SVHarmonyInit()
        {
            new Harmony("dorbo.shamblervariants").PatchAll();
        }
    }
}
