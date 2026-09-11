using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(InvisibilityUtility), "GetAlpha")]
    public static class InvisibilityUtility_GetAlpha_Patch
    {
        private static readonly Dictionary<MutantDef, float> cache = new Dictionary<MutantDef, float>();

        public static void Postfix(Pawn pawn, ref float __result)
        {
            if (__result < 1f || pawn == null || pawn.mutant == null)
            {
                return;
            }
            MutantDef def = pawn.mutant.Def;
            float alpha;
            if (!cache.TryGetValue(def, out alpha))
            {
                SVMutantSkin ext = def.GetModExtension<SVMutantSkin>();
                alpha = ext != null ? ext.alpha : 1f;
                cache[def] = alpha;
            }
            if (alpha < 1f)
            {
                __result = alpha;
            }
        }
    }
}
