using HarmonyLib;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(Pawn), "IsShambler", MethodType.Getter)]
    public static class Pawn_IsShambler_Patch
    {
        public static void Postfix(Pawn __instance, ref bool __result)
        {
            if (!__result)
            {
                __result = SVShamblerMutant.OurDef(__instance) != null;
            }
        }
    }
}
