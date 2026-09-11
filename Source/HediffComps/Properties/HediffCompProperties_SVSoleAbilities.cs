using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVSoleAbilities : HediffCompProperties
    {
        public int checkInterval = 600;

        public HediffCompProperties_SVSoleAbilities()
        {
            compClass = typeof(HediffComp_SVSoleAbilities);
        }
    }
}
