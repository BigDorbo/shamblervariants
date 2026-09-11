using Verse;

namespace ShamblerVariants
{
    public abstract class HediffComp_SV<T> : HediffComp where T : HediffCompProperties
    {
        public T Props
        {
            get { return (T)props; }
        }
    }
}
