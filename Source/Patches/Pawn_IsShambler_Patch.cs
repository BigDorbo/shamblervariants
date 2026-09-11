using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(Pawn), "IsShambler", MethodType.Getter)]
    public static class Pawn_IsShambler_Patch
    {
        private static readonly Dictionary<MutantDef, bool> cache = new Dictionary<MutantDef, bool>();

        public static void Postfix(Pawn __instance, ref bool __result)
        {
            if (__result || __instance.mutant == null)
            {
                return;
            }
            MutantDef def = __instance.mutant.Def;
            bool ours;
            if (!cache.TryGetValue(def, out ours))
            {
                ours = def.GetModExtension<SVShamblerMutant>() != null;
                cache[def] = ours;
            }
            __result = ours;
        }
    }
}
