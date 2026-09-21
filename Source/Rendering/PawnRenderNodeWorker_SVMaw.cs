using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class PawnRenderNodeWorker_SVMaw : PawnRenderNodeWorker_FlipWhenCrawling
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            if (!base.CanDrawNow(node, parms))
            {
                return false;
            }
            Pawn p = parms.pawn;
            return WindingUp(p) != IsClosedArt(node);
        }

        private static bool WindingUp(Pawn p)
        {
            Verb_CastAbility verb = SVCast.WarmingUp(p);
            return verb != null && verb.Ability.def == SVDefOf.SV_LatcherChomp;
        }

        private static bool IsClosedArt(PawnRenderNode node)
        {
            return ((PawnRenderNodeProperties_SVMaw)node.Props).closed;
        }
    }
}
