using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(InvisibilityUtility), "GetAlpha")]
    public static class InvisibilityUtility_GetAlpha_Patch
    {
        public static void Postfix(Pawn pawn, ref float __result)
        {
            if (__result < 1f || pawn == null || pawn.mutant == null)
            {
                return;
            }
            MutantDef def = pawn.mutant.Def;
            if (def == null)
            {
                return;
            }
            SVMutantSkin ext = def.GetModExtension<SVMutantSkin>();
            if (ext != null && ext.alpha < 1f)
            {
                __result = ext.alpha;
            }
        }
    }
}
