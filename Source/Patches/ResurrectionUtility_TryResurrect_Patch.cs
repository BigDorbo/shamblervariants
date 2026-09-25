using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(ResurrectionUtility), "TryResurrect")]
    public static class ResurrectionUtility_TryResurrect_Patch
    {
        public static void Postfix(Pawn pawn, bool __result)
        {
            if (__result && SVShamblerMutant.OurDef(pawn) == null)
            {
                SVVariantKit.Strip(pawn);
            }
        }
    }
}
