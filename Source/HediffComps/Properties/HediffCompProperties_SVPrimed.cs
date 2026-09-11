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
    }
}
