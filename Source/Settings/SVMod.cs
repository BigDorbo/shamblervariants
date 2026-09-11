using UnityEngine;
using Verse;

namespace ShamblerVariants
{
    public class SVMod : Mod
    {
        public static readonly bool Biotech = ModsConfig.BiotechActive;
        public static SVSettings Settings;

        public SVMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<SVSettings>();
        }

        public override string SettingsCategory()
        {
            return Biotech ? "Shambler Variants" : "";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            bool baselinerBefore = Settings.baselinerOnly;
            bool unlistedBefore = Settings.unlistedXenotypes;
            list.CheckboxLabeled("Only baseliner variants", ref Settings.baselinerOnly, null, 0f, 1f);
            list.CheckboxLabeled("Unsupported xenotype variants", ref Settings.unlistedXenotypes, null, 0f, 1f);
            if (Settings.baselinerOnly && !baselinerBefore)
            {
                Settings.unlistedXenotypes = false;
            }
            if (Settings.unlistedXenotypes && !unlistedBefore)
            {
                Settings.baselinerOnly = false;
            }
            list.End();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            SVXenotypeInjector.Apply();
        }
    }
}
