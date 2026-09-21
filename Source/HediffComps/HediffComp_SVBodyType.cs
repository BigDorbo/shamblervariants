using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVBodyType : HediffComp_SV<HediffCompProperties_SVBodyType>
    {

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            if (p.story == null || Props.bodyType == null
                || p.story.bodyType == Props.bodyType)
            {
                return;
            }
            p.story.bodyType = Props.bodyType;
            p.Drawer.renderer.SetAllGraphicsDirty();
        }
    }
}
