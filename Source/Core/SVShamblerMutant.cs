using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class SVShamblerMutant : DefModExtension
    {
        public static bool Marks(MutantDef def)
        {
            return def != null && def.HasModExtension<SVShamblerMutant>();
        }

        public static MutantDef OurDef(Pawn p)
        {
            Pawn_MutantTracker mutant = p.mutant;
            return mutant != null && Marks(mutant.Def) ? mutant.Def : null;
        }
    }
}
