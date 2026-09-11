using RimWorld;
using Verse;
using Verse.AI;

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
            Stance_Warmup stance = p.stances.curStance as Stance_Warmup;
            if (stance == null)
            {
                return false;
            }
            Verb_CastAbility verb = stance.verb as Verb_CastAbility;
            return verb != null && verb.Ability != null
                && verb.Ability.def == SVDefOf.SV_LatcherChomp;
        }

        private static bool IsClosedArt(PawnRenderNode node)
        {
            return ((PawnRenderNodeProperties_SVMaw)node.Props).closed;
        }
    }
}
