using HarmonyLib;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [HarmonyPatch(typeof(Pawn), "DeadlifeDustFaction", MethodType.Getter)]
    public static class Pawn_DeadlifeDustFaction_Patch
    {
        public static void Postfix(Pawn __instance, ref Faction __result, Faction ___deadlifeDustFaction, int ___deadlifeDustFactionTick)
        {
            if (___deadlifeDustFaction != null && GenTicks.TicksGame - ___deadlifeDustFactionTick < MapComponent_SVDeadlifeClaims.MarkTicks)
            {
                return;
            }
            Map map = __instance.MapHeld;
            if (map == null)
            {
                return;
            }
            Faction owner = map.GetComponent<MapComponent_SVDeadlifeClaims>().OwnerAt(__instance.PositionHeld);
            if (owner != null)
            {
                __result = owner;
            }
        }
    }
}
