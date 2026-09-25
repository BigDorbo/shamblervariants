using System.Collections.Generic;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVGrowths : HediffComp_SV<HediffCompProperties_SVGrowths>
    {
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            List<BodyPartRecord> parts = new List<BodyPartRecord>(p.RaceProps.body.GetPartsWithDef(Props.InstallPart));
            if (parts.Count == 0)
            {
                return;
            }
            parts.Shuffle();
            int want = Props.countRange.RandomInRange;
            if (want > parts.Count)
            {
                want = parts.Count;
            }
            for (int i = 0; i < want; i++)
            {
                p.health.AddHediff(Props.growth, parts[i], null, null);
            }
        }
    }
}
