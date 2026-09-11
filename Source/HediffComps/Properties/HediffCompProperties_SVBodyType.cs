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
    }
}
