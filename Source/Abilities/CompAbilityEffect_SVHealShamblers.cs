using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVHealShamblers : CompAbilityEffect_SV<CompProperties_SVAbilityHealShamblers>
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn caster = parent.pawn;
            Map map = caster.Map;
            IntVec3 center = target.Cell;
            if (Props.areaFleck != null)
            {
                FleckMaker.Static(center, map, Props.areaFleck, Props.areaFleckScale);
            }
            SVCast.Sound(Props.castSound, caster);
            if (caster.Faction == null)
            {
                return;
            }
            List<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(caster.Faction);
            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn other = pawns[i];
                if (!other.IsShambler || !other.Position.InHorDistOf(center, Props.radius))
                {
                    continue;
                }
                bool healed = Props.healAmount > 0f
                    && SVHeal.HealWounds(other, Props.healAmount, Props.maxWoundsPerPawn) > 0;
                if (Props.Mark(other) || healed)
                {
                    SVCast.MetaIcon(Props.targetFleck, other.Position, map);
                }
            }
        }
    }
}
