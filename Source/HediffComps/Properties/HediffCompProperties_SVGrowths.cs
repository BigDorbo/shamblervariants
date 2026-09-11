using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVGrowths : HediffCompProperties
    {
        public HediffDef growth;
        public BodyPartDef partDef;
        public IntRange countRange = new IntRange(1, 1);

        public HediffCompProperties_SVGrowths()
        {
            compClass = typeof(HediffComp_SVGrowths);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (growth == null)
            {
                yield return "SV growths needs a growth hediff";
            }
        }
    }
}
