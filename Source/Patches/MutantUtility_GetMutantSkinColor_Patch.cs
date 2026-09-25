using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(MutantUtility), "GetMutantSkinColor")]
    public static class MutantUtility_GetMutantSkinColor_Patch
    {
        public static void Postfix(Pawn pawn, ref Color __result)
        {
            MutantDef def = SVShamblerMutant.OurDef(pawn);
            if (def != null && def.skinColorOverride != null && pawn.mutant.HasTurned)
            {
                __result = MutantUtility.GetShamblerColor(def.skinColorOverride.Value);
            }
        }
    }
}
