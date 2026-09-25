using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public abstract class CompProperties_SVAbilityMark : CompProperties_AbilityEffect
    {
        public HediffDef applyHediff;
        public int hediffDurationTicks;

        public bool Mark(Pawn target)
        {
            if (applyHediff == null)
            {
                return false;
            }
            Hediff mark = target.health.GetOrAddHediff(applyHediff, null, null, null);
            if (hediffDurationTicks > 0)
            {
                HediffComp_Disappears dis = mark.TryGetComp<HediffComp_Disappears>();
                if (dis.ticksToDisappear < hediffDurationTicks)
                {
                    dis.SetDuration(hediffDurationTicks);
                }
            }
            return true;
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (applyHediff != null && hediffDurationTicks > 0 && !applyHediff.HasComp(typeof(HediffComp_Disappears)))
            {
                yield return "SV hediffDurationTicks needs an applyHediff with HediffCompProperties_Disappears";
            }
        }
    }
}
