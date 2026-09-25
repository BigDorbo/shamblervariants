using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffComp_SVBodyType : HediffComp_SV<HediffCompProperties_SVBodyType>
    {
        private BodyTypeDef original;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            if (p.story == null || p.story.bodyType == Props.bodyType)
            {
                return;
            }
            original = p.story.bodyType;
            Swap(p, Props.bodyType);
        }

        public override void CompPostPostRemoved()
        {
            Pawn p = Pawn;
            if (original == null || p.story == null || p.story.bodyType != Props.bodyType)
            {
                return;
            }
            Swap(p, original);
        }

        private static void Swap(Pawn p, BodyTypeDef bodyType)
        {
            p.story.bodyType = bodyType;
            PawnRenderer renderer = p.Drawer.renderer;
            renderer.SetAllGraphicsDirty();
            LongEventHandler.ExecuteWhenFinished(delegate { renderer.ShamblerScarDrawer.ClearCache(); });
        }

        public override void CompExposeData()
        {
            Scribe_Defs.Look<BodyTypeDef>(ref original, "SV_originalBodyType");
        }
    }
}
