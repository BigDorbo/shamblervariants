using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVCasterDrive : HediffCompProperties
    {
        public int checkInterval = 240;
        public List<SVAbilityCast> casts;

        public HediffCompProperties_SVCasterDrive()
        {
            compClass = typeof(HediffComp_SVCasterDrive);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (checkInterval <= 0)
            {
                yield return "SV caster drive needs a checkInterval above 0";
            }
            if (casts == null || casts.Count == 0)
            {
                yield return "SV caster drive needs at least one cast";
                yield break;
            }
            for (int i = 0; i < casts.Count; i++)
            {
                SVAbilityCast cast = casts[i];
                if (cast == null || cast.ability == null)
                {
                    yield return "SV caster drive cast entry " + i + " has no ability";
                }
                else if (!cast.selfCast && !cast.targetWoundedShamblers && !cast.targetHostiles && !cast.targetCorpses && !cast.targetGraves)
                {
                    yield return "SV caster drive cast " + cast.ability.defName + " has no target mode";
                }
            }
            List<PawnKindDef> kinds = DefDatabase<PawnKindDef>.AllDefsListForReading;
            for (int k = 0; k < kinds.Count; k++)
            {
                PawnKindDef kind = kinds[k];
                if (!Carries(kind, parentDef))
                {
                    continue;
                }
                for (int i = 0; i < casts.Count; i++)
                {
                    AbilityDef ability = casts[i] != null ? casts[i].ability : null;
                    if (ability == null)
                    {
                        continue;
                    }
                    if (kind.abilities == null || !kind.abilities.Contains(ability))
                    {
                        yield return "SV caster drive cast " + ability.defName + " is not in the abilities of " + kind.defName;
                    }
                }
            }
        }

        private static bool Carries(PawnKindDef kind, HediffDef def)
        {
            List<StartingHediff> starting = kind.startingHediffs;
            if (starting == null)
            {
                return false;
            }
            for (int i = 0; i < starting.Count; i++)
            {
                if (starting[i].def == def)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
