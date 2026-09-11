using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public static class SVHeal
    {
        public static bool HasInjury(Pawn p)
        {
            List<Hediff> list = p.health.hediffSet.hediffs;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Hediff_Injury)
                {
                    return true;
                }
            }
            return false;
        }

        public static int HealWounds(Pawn p, float amount, int maxWounds)
        {
            List<Hediff> list = p.health.hediffSet.hediffs;
            int healed = 0;
            for (int i = list.Count - 1; i >= 0 && healed < maxWounds; i--)
            {
                Hediff_Injury injury = list[i] as Hediff_Injury;
                if (injury != null)
                {
                    injury.Heal(amount);
                    healed++;
                }
            }
            return healed;
        }
    }
}
