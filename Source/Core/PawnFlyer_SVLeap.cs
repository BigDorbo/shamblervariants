using RimWorld;
using UnityEngine;
using Verse;

namespace ShamblerVariants
{
    public class PawnFlyer_SVLeap : PawnFlyer
    {
        public const float ShakeMagnitude = 1.2f;

        protected override void TickInterval(int delta)
        {
            if (FlyingThing == null)
            {
                Log.Error(this + " at " + Position + " has no pawn inside; destroying it.");
                Destroy(DestroyMode.Vanish);
                return;
            }
            base.TickInterval(delta);
        }

        protected override void RespawnPawn()
        {
            Vector3 dest = DestinationPos;
            Map map = Map;
            base.RespawnPawn();
            Find.CameraDriver.shaker.DoShake(ShakeMagnitude);
            if (map != null)
            {
                FleckMaker.ThrowDustPuff(dest, map, 2.6f);
                FleckMaker.ThrowDustPuff(dest, map, 1.8f);
            }
        }
    }
}
