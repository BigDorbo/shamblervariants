using RimWorld;
using Verse;

namespace ShamblerVariants
{
    [StaticConstructorOnStartup]
    public class Thing_SVTanglerKnot : ThingWithComps
    {
        public Pawn caster;
        public Pawn victim;
        public HediffDef pinHediff;
        public float maxLength = 8f;

        public const float VineWidth = 0.054f;

        private static readonly UnityEngine.Material VineCoreMat =
            SolidColorMaterials.SimpleSolidColorMaterial(new UnityEngine.Color(0.30f, 0.51f, 0.21f, 1f), false);

        private static readonly UnityEngine.Material VineEdgeMat =
            SolidColorMaterials.SimpleSolidColorMaterial(new UnityEngine.Color(0.17f, 0.31f, 0.12f, 1f), false);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref caster, "SV_tetherCaster", false);
            Scribe_References.Look<Pawn>(ref victim, "SV_tetherVictim", false);
            Scribe_Defs.Look<HediffDef>(ref pinHediff, "SV_tetherPin");
            Scribe_Values.Look<float>(ref maxLength, "SV_tetherMaxLength", 8f, false);
        }

        private bool EndsValid()
        {
            if (victim == null || victim.Dead || !victim.Spawned || victim.Map != Map)
            {
                return false;
            }
            if (pinHediff != null && !victim.health.hediffSet.HasHediff(pinHediff, false))
            {
                return false;
            }
            return (Position - victim.Position).LengthHorizontalSquared <= maxLength * maxLength;
        }

        public override void TickRare()
        {
            base.TickRare();
            if (!EndsValid())
            {
                Destroy(DestroyMode.Vanish);
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            Pawn v = victim;
            HediffDef pin = pinHediff;
            base.Destroy(mode);
            if (v != null && !v.Dead && pin != null)
            {
                Hediff h = v.health.hediffSet.GetFirstHediffOfDef(pin, false);
                if (h != null)
                {
                    v.health.RemoveHediff(h);
                }
            }
        }

        protected override void DrawAt(UnityEngine.Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            if (victim == null || !victim.Spawned)
            {
                return;
            }
            SVTether.Draw(DrawPos, victim.DrawPos, VineEdgeMat, VineCoreMat,
                VineWidth, thingIDNumber % 360);
        }
    }
}
