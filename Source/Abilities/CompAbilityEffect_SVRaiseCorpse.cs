using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class CompAbilityEffect_SVRaiseCorpse : CompAbilityEffect_SVGated<CompProperties_SVAbilityRaiseCorpse>
    {

        public Corpse RaisableCorpse(LocalTargetInfo target)
        {
            Thing thing = target.Thing;
            if (Props.gravesOnly && !(thing is Building_Casket))
            {
                return null;
            }
            if (Props.corpsesOnly && !(thing is Corpse))
            {
                return null;
            }
            if (thing == null || thing.Destroyed)
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
            if (corpse == null)
            {
                return null;
            }
            return MutantUtility.CanResurrectAsShambler(corpse, true) ? corpse : null;
        }

        protected override bool ReadyFor(LocalTargetInfo target)
        {
            return RaisableCorpse(target) != null;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Corpse corpse = RaisableCorpse(target);
            if (corpse == null)
            {
                return;
            }
            Building_Casket casket = target.Thing as Building_Casket;
            if (casket != null && casket.HasAnyContents)
            {
                IntVec3 at = casket.Position;
                Map cm = casket.Map;
                casket.EjectContents();
                if (cm != null)
                {
                    FleckMaker.ThrowDustPuff(at, cm, Props.digFleckScale);
                    FleckMaker.ThrowDustPuff(at, cm, Props.digFleckScale * 0.7f);
                }
            }
            if (Props.exhumeOnly)
            {
                return;
            }
            Map map = corpse.MapHeld;
            if (map == null)
            {
                return;
            }
            GasUtility.AddDeadifeGas(corpse.PositionHeld, map, parent.pawn.Faction,
                Props.cellsToFill * 255);
        }
    }
}
