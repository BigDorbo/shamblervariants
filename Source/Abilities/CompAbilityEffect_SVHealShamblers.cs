using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVHealShamblers : CompAbilityEffect_SV<CompProperties_SVAbilityHealShamblers>
    {

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn caster = parent.pawn;
            Map map = caster.MapHeld;
            if (map == null)
            {
                return;
            }
            IntVec3 center = target.Cell;
            if (Props.areaFleck != null)
            {
                FleckMaker.Static(center, map, Props.areaFleck, Props.areaFleckScale);
            }
            if (Props.castSound != null)
            {
                Props.castSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(caster), MaintenanceType.None));
            }
            if (caster.Faction == null)
            {
                return;
            }
            List<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(caster.Faction);
            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn other = pawns[i];
                if (other.Dead || !other.IsShambler)
                {
                    continue;
                }
                if (!SVScan.WithinSquared(other.Position, center, Props.radius))
                {
                    continue;
                }
                bool touched = false;
                if (Props.healAmount > 0f
                    && SVHeal.HealWounds(other, Props.healAmount, Props.maxWoundsPerPawn) > 0)
                {
                    touched = true;
                }
                if (Props.applyHediff != null)
                {
                    Hediff existing = other.health.hediffSet.GetFirstHediffOfDef(Props.applyHediff, false);
                    if (existing == null)
                    {
                        existing = other.health.AddHediff(Props.applyHediff, null, null, null);
                    }
                    if (Props.hediffDurationTicks > 0)
                    {
                        HediffComp_Disappears dis = existing.TryGetComp<HediffComp_Disappears>();
                        if (dis != null && dis.ticksToDisappear < Props.hediffDurationTicks)
                        {
                            dis.ticksToDisappear = Props.hediffDurationTicks;
                        }
                    }
                    touched = true;
                }
                if (touched && Props.targetFleck != null)
                {
                    FleckMaker.ThrowMetaIcon(other.Position, map, Props.targetFleck, 0.42f);
                }
            }
        }
    }
}
