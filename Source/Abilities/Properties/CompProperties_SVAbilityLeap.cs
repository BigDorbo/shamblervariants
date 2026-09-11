using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityLeap : CompProperties_SVRelocate
    {
        public ThingDef flyerDef;
        public HediffDef requiredHediff;
        public SoundDef leapSound;
        public SoundDef landSound;

        public CompProperties_SVAbilityLeap()
        {
            compClass = typeof(CompAbilityEffect_SVLeap);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (flyerDef == null)
            {
                yield return "SV leap needs a flyerDef";
            }
            else if (!typeof(PawnFlyer_SVLeap).IsAssignableFrom(flyerDef.thingClass))
            {
                yield return "SV leap flyerDef must use thingClass PawnFlyer_SVLeap";
            }
        }
    }
}
