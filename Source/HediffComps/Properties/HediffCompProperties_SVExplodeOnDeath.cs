using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class HediffCompProperties_SVExplodeOnDeath : HediffCompProperties
    {
        public float radius = 2.4f;
        public int damAmount = 12;
        public DamageDef damageDef;
        public float fireChance = 0.9f;
        public ThingDef spreadFilth;
        public float spreadChance;
        public int spreadCount = 1;
        public bool gore;
        public int goreFilthCount;
        public float goreFilthRadius = 1.9f;
        public ThingDef goreFilthDef;
        public bool destroyCorpse;
        public bool detonateWhenBurning;
        public int burnCheckInterval = 30;

        public HediffCompProperties_SVExplodeOnDeath()
        {
            compClass = typeof(HediffComp_SVExplodeOnDeath);
        }

        public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (damageDef == null && (radius > 0f || detonateWhenBurning))
            {
                yield return "SV explode on death needs a damageDef";
            }
            if (detonateWhenBurning && burnCheckInterval <= 0)
            {
                yield return "SV explode on death needs a burnCheckInterval above 0";
            }
            if (spreadChance > 0f && spreadFilth == null)
            {
                yield return "SV explode on death spreadChance needs a spreadFilth";
            }
            if (gore && goreFilthCount > 0 && goreFilthDef == null)
            {
                yield return "SV explode on death goreFilthCount needs a goreFilthDef";
            }
        }
    }
}
