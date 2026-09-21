using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVSoleAbilities : HediffComp_SV<HediffCompProperties_SVSoleAbilities>
    {

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Strip();
            ApplySkin();
        }

        private void ApplySkin()
        {
            Pawn p = Pawn;
            if (p.story == null || p.mutant == null)
            {
                return;
            }
            MutantDef def = p.mutant.Def;
            if (def == null)
            {
                return;
            }
            SVMutantSkin skin = def.GetModExtension<SVMutantSkin>();
            if (skin == null || skin.color.a <= 0f)
            {
                return;
            }
            p.story.skinColorOverride = skin.color;
            p.Drawer.renderer.SetAllGraphicsDirty();
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            Pawn p = Pawn;
            if (p.Dead)
            {
                return;
            }
            if (!p.IsHashIntervalTick(Props.checkInterval, delta))
            {
                return;
            }
            Strip();
        }

        private void Strip()
        {
            Pawn p = Pawn;
            if (p.abilities == null)
            {
                return;
            }
            List<Ability> held = p.abilities.abilities;
            List<AbilityDef> keep = p.kindDef.abilities;
            for (int i = held.Count - 1; i >= 0; i--)
            {
                AbilityDef def = held[i].def;
                if (keep != null && keep.Contains(def))
                {
                    continue;
                }
                p.abilities.RemoveAbility(def);
            }
        }
    }
}
