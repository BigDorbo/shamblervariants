using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [StaticConstructorOnStartup]
    public static class SVXenotypeInjector
    {
        private const float UnlistedWeight = 0.5f;
        private static readonly FieldInfo chancesField = SVReflect.Field(typeof(XenotypeSet), "xenotypeChances");
        private static readonly Dictionary<PawnKindDef, List<XenotypeChance>> listed = new Dictionary<PawnKindDef, List<XenotypeChance>>();

        static SVXenotypeInjector()
        {
            Apply();
        }

        public static void Apply()
        {
            if (!SVMod.Biotech)
            {
                return;
            }
            List<PawnKindDef> kinds = DefDatabase<PawnKindDef>.AllDefsListForReading;
            for (int i = 0; i < kinds.Count; i++)
            {
                PawnKindDef kind = kinds[i];
                if (!SVShamblerMutant.Marks(kind.mutant))
                {
                    continue;
                }
                if (kind.xenotypeSet == null)
                {
                    kind.xenotypeSet = new XenotypeSet();
                }
                List<XenotypeChance> chances = (List<XenotypeChance>)chancesField.GetValue(kind.xenotypeSet);
                List<XenotypeChance> original;
                if (!listed.TryGetValue(kind, out original))
                {
                    original = new List<XenotypeChance>(chances);
                    listed[kind] = original;
                }
                chances.Clear();
                if (SVMod.Settings.baselinerOnly)
                {
                    continue;
                }
                chances.AddRange(original);
                if (!SVMod.Settings.unlistedXenotypes)
                {
                    continue;
                }
                List<XenotypeDef> all = DefDatabase<XenotypeDef>.AllDefsListForReading;
                for (int j = 0; j < all.Count; j++)
                {
                    XenotypeDef x = all[j];
                    if (x != XenotypeDefOf.Baseliner && x.canGenerateAsCombatant && !Listed(original, x))
                    {
                        chances.Add(new XenotypeChance(x, UnlistedWeight));
                    }
                }
            }
        }

        private static bool Listed(List<XenotypeChance> chances, XenotypeDef x)
        {
            for (int i = 0; i < chances.Count; i++)
            {
                if (chances[i].xenotype == x)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
