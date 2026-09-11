using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(MutantUtility), "ResurrectAsShambler")]
    public static class MutantUtility_ResurrectAsShambler_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            if (pawn.mutant == null)
            {
                return;
            }
            MutantDef ours = pawn.kindDef.mutant;
            if (ours == null || ours.GetModExtension<SVShamblerMutant>() == null
                || pawn.mutant.Def == ours)
            {
                return;
            }
            pawn.mutant = new Pawn_MutantTracker(pawn, ours, pawn.mutant.rotStage);
            List<StartingHediff> starting = pawn.kindDef.startingHediffs;
            if (starting != null)
            {
                for (int i = 0; i < starting.Count; i++)
                {
                    HediffDef def = starting[i].def;
                    if (def != null && !pawn.health.hediffSet.HasHediff(def, false))
                    {
                        pawn.health.AddHediff(def, null, null, null);
                    }
                }
            }
            List<AbilityDef> abilities = pawn.kindDef.abilities;
            if (abilities != null && pawn.abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    pawn.abilities.GainAbility(abilities[i]);
                }
            }
        }
    }
}
