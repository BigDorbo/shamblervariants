using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVRaiseCorpse : CompAbilityEffect_SVGated<CompProperties_SVAbilityRaiseCorpse>
    {
        private Corpse RaisableCorpse(LocalTargetInfo target)
        {
            if (!Props.exhumeOnly && parent.pawn.Faction == null)
            {
                return null;
            }
            Thing thing = target.Thing;
            if (thing == null || thing.Destroyed
                || (Props.gravesOnly && !(thing is Building_Casket))
                || (Props.corpsesOnly && !(thing is Corpse)))
            {
                return null;
            }
            Corpse corpse = thing as Corpse;
            if (corpse == null)
            {
                Building_Casket casket = thing as Building_Casket;
                if (casket == null || !casket.HasAnyContents)
                {
                    return null;
                }
                corpse = casket.ContainedThing as Corpse;
            }
            return corpse != null && MutantUtility.CanResurrectAsShambler(corpse, true) ? corpse : null;
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            return RaisableCorpse(target) != null;
        }

        protected override void Cast(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Corpse corpse = RaisableCorpse(target);
            Building_Casket casket = target.Thing as Building_Casket;
            if (casket != null)
            {
                IntVec3 at = casket.Position;
                Map cm = casket.Map;
                casket.EjectContents();
                FleckMaker.ThrowDustPuff(at, cm, Props.digFleckScale);
                FleckMaker.ThrowDustPuff(at, cm, Props.digFleckScale * 0.7f);
            }
            if (Props.exhumeOnly)
            {
                return;
            }
            Map map = corpse.MapHeld;
            IntVec3 cell = corpse.PositionHeld;
            Faction faction = parent.pawn.Faction;
            GasUtility.AddDeadifeGas(cell, map, faction, Props.cellsToFill * GasGrid.MaxGasPerCell);
            MapComponent_SVDeadlifeClaims.Add(map, cell, faction, Props.claimRadius);
        }
    }
}
