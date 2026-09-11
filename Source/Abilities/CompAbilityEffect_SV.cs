using RimWorld;

namespace ShamblerVariants
{
    public abstract class CompAbilityEffect_SV<T> : CompAbilityEffect where T : CompProperties_AbilityEffect
    {
        public new T Props
        {
            get { return (T)props; }
        }
    }
}
