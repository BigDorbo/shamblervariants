using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [StaticConstructorOnStartup]
    public static class SVVariantKit
    {
        public static readonly HashSet<HediffDef> Hediffs = new HashSet<HediffDef>();
        public static readonly HashSet<AbilityDef> Abilities = new HashSet<AbilityDef>();

        private static readonly Assembly Ours = typeof(SVVariantKit).Assembly;

        static SVVariantKit()
        {
            List<HediffDef> hediffs = DefDatabase<HediffDef>.AllDefsListForReading;
            for (int i = 0; i < hediffs.Count; i++)
            {
                HediffDef def = hediffs[i];
                if (IsOurs(def.hediffClass) || AnyOurs(def.comps, delegate(HediffCompProperties c) { return c.compClass; }))
                {
                    Hediffs.Add(def);
                }
            }
            List<AbilityDef> abilities = DefDatabase<AbilityDef>.AllDefsListForReading;
            for (int i = 0; i < abilities.Count; i++)
            {
                AbilityDef def = abilities[i];
                if (IsOurs(def.abilityClass) || AnyOurs(def.comps, delegate(AbilityCompProperties c) { return c.compClass; }))
                {
                    Abilities.Add(def);
                }
            }
            List<PawnKindDef> kinds = DefDatabase<PawnKindDef>.AllDefsListForReading;
            for (int i = 0; i < kinds.Count; i++)
            {
                PawnKindDef kind = kinds[i];
                if (!SVShamblerMutant.Marks(kind.mutant) || kind.abilities == null)
                {
                    continue;
                }
                List<AbilityDef> whitelist = kind.mutant.abilityWhitelist;
                for (int j = 0; j < kind.abilities.Count; j++)
                {
                    if (!whitelist.Contains(kind.abilities[j]))
                    {
                        whitelist.Add(kind.abilities[j]);
                    }
                }
            }
        }

        private static bool IsOurs(Type type)
        {
            return type != null && type.Assembly == Ours;
        }

        private static bool AnyOurs<T>(List<T> comps, Func<T, Type> compClass)
        {
            if (comps == null)
            {
                return false;
            }
            for (int i = 0; i < comps.Count; i++)
            {
                if (IsOurs(compClass(comps[i])))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Strip(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = hediffs.Count - 1; i >= 0; i--)
            {
                if (Hediffs.Contains(hediffs[i].def))
                {
                    pawn.health.RemoveHediff(hediffs[i]);
                }
            }
            if (pawn.abilities == null)
            {
                return;
            }
            List<Ability> abilities = pawn.abilities.abilities;
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                if (Abilities.Contains(abilities[i].def))
                {
                    pawn.abilities.RemoveAbility(abilities[i].def);
                }
            }
        }
    }
}
