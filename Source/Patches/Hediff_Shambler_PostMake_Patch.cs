using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(Hediff_Shambler), "PostMake")]
    [HarmonyAfter("com.alphagenes")]
    [HarmonyPriority(Priority.Low)]
    public static class Hediff_Shambler_PostMake_Patch
    {
        private static readonly Type foreignGene = GenTypes.GetTypeInAnyAssembly("VEF.Genes.Gene_Shambler", null);
        private static readonly List<Gene> strip = new List<Gene>();

        public static void Postfix(Hediff_Shambler __instance)
        {
            if (foreignGene == null)
            {
                return;
            }
            Pawn pawn = __instance.pawn;
            if (pawn.genes == null || pawn.mutant.Def.GetModExtension<SVShamblerMutant>() == null)
            {
                return;
            }
            strip.Clear();
            List<Gene> genes = pawn.genes.GenesListForReading;
            for (int i = 0; i < genes.Count; i++)
            {
                if (genes[i].def.geneClass == foreignGene)
                {
                    strip.Add(genes[i]);
                }
            }
            for (int i = 0; i < strip.Count; i++)
            {
                pawn.genes.RemoveGene(strip[i]);
            }
        }
    }
}
