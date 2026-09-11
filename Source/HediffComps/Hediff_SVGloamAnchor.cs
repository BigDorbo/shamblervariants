using Verse;

namespace ShamblerVariants
{
    public class Hediff_SVGloamAnchor : HediffWithComps
    {
        public Pawn echo;

        public bool EchoActive
        {
            get { return echo != null && !echo.Dead && !echo.Destroyed; }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref echo, "SV_echo", false);
        }
    }
}
