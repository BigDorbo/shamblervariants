using Verse;

namespace ShamblerVariants
{
    public class SVSettings : ModSettings
    {
        public bool baselinerOnly;
        public bool unlistedXenotypes;

        public override void ExposeData()
        {
            Scribe_Values.Look<bool>(ref baselinerOnly, "SV_baselinerOnly", false, false);
            Scribe_Values.Look<bool>(ref unlistedXenotypes, "SV_unlistedXenotypes", false, false);
        }
    }
}
