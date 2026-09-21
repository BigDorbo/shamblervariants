using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVPrimed : HediffCompProperties
    {
        public int durationTicks = 120;
        public SoundDef startSound;
        public EffecterDef effecter;
        public DamageDef damageDef;

        public HediffCompProperties_SVPrimed()
        {
            compClass = typeof(HediffComp_SVPrimed);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (damageDef == null)
            {
                yield return "SV primed needs a damageDef";
            }
        }
    }
}
