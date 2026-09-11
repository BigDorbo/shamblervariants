using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompProperties_SVAbilityBlink : CompProperties_SVRelocate
    {
        public float arriveScanRadius = 2.9f;
        public int stunTicks = 90;
        public SoundDef arriveSound;
        public EffecterDef targetEffecter;
        public bool cloneOnArrive;
        public float cloneHealthFraction = 0.5f;
        public HediffDef echoHediff;
        public HediffDef anchorHediff;

        public CompProperties_SVAbilityBlink()
        {
            compClass = typeof(CompAbilityEffect_SVBlink);
        }

        public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (cloneOnArrive && (echoHediff == null || anchorHediff == null))
            {
                yield return "SV blink with cloneOnArrive needs an echoHediff and an anchorHediff";
            }
            else if (cloneOnArrive && anchorHediff.hediffClass != typeof(Hediff_SVGloamAnchor))
            {
                yield return "SV blink anchorHediff must use hediffClass Hediff_SVGloamAnchor";
            }
        }
    }
}
