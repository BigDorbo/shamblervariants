using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(Pawn), "IsShambler", MethodType.Getter)]
    public static class Pawn_IsShambler_Patch
    {
        public static void Postfix(Pawn __instance, ref bool __result)
        {
            if (__result || __instance.mutant == null)
            {
                return;
            }
            MutantDef def = __instance.mutant.Def;
            if (def == null)
            {
                return;
            }
            __result = def.GetModExtension<SVShamblerMutant>() != null;
        }
    }
}
