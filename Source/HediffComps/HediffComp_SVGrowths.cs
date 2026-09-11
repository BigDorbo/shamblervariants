using System.Collections.Generic;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVGrowths : HediffComp_SV<HediffCompProperties_SVGrowths>
    {

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            BodyPartDef partDef = Props.partDef != null
                ? Props.partDef
                : Props.growth.defaultInstallPart;
            if (partDef == null)
            {
                return;
            }
            List<BodyPartRecord> parts = new List<BodyPartRecord>();
            List<BodyPartRecord> all = p.RaceProps.body.AllParts;
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].def == partDef)
                {
                    parts.Add(all[i]);
                }
            }
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
