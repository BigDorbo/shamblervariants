using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityTether : CompProperties_AbilityEffect
    {
        public ThingDef tetherDef;
        public HediffDef pinHediff;
        public float maxLength = 8f;
        public float knotPlacementRadius = 1.9f;
        public FleckDef targetFleck;
        public SoundDef castSound;

        public CompProperties_SVAbilityTether()
        {
            compClass = typeof(CompAbilityEffect_SVTether);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (tetherDef == null)
            {
                yield return "SV tether needs a tetherDef";
            }
            else if (!typeof(Thing_SVTanglerKnot).IsAssignableFrom(tetherDef.thingClass))
            {
                yield return "SV tether tetherDef must use thingClass Thing_SVTanglerKnot";
            }
            if (pinHediff == null)
            {
                yield return "SV tether needs a pinHediff";
            }
        }
    }
}
