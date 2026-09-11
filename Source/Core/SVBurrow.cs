using RimWorld;
using Verse;
using Verse.Sound;

namespace ShamblerVariants
{
    public static class SVBurrow
    {
        public static void BreakGround(Pawn p, SoundDef breakSound)
        {
            Map map = p.Map;
            FleckMaker.ThrowDustPuff(p.Position, map, 2.2f);
            FleckMaker.ThrowDustPuff(p.Position, map, 1.6f);
            FleckMaker.ThrowDustPuff(p.Position, map, 1f);
            if (breakSound != null)
            {
                breakSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(p), MaintenanceType.None));
            }
        }
    }
}
