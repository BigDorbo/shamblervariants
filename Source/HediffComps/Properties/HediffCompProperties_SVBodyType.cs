using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVBodyType : HediffCompProperties
    {
        public BodyTypeDef bodyType;

        public HediffCompProperties_SVBodyType()
        {
            compClass = typeof(HediffComp_SVBodyType);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (bodyType == null)
            {
                yield return "SV body type needs a bodyType";
            }
        }
    }
}
