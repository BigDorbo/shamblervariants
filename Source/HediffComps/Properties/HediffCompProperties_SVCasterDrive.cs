using System.Collections.Generic;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVCasterDrive : HediffCompProperties
    {
        public int checkInterval = 240;
        public List<SVAbilityCast> casts;

        public HediffCompProperties_SVCasterDrive()
        {
            compClass = typeof(HediffComp_SVCasterDrive);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (casts == null || casts.Count == 0)
            {
                yield return "SV caster drive needs at least one cast";
                yield break;
            }
            for (int i = 0; i < casts.Count; i++)
            {
                if (casts[i] == null || casts[i].ability == null)
                {
                    yield return "SV caster drive cast entry " + i + " has no ability";
                }
            }
        }
    }
}
