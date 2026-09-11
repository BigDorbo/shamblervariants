using RimWorld;
using Verse;

namespace ShamblerVariants
{
    public class SVAbilityCast
    {
        public AbilityDef ability;
        public float searchRadius = 49.9f;
        public bool targetWoundedShamblers;
        public bool targetHostiles;
        public bool selfCast;
        public bool requireHostileNear;
        public bool requireEnemyTarget;
        public float hostileNearRadius = 5.9f;
        public HediffDef skipIfTargetHas;
        public bool skipToTarget;
        public bool targetCorpses;
        public bool targetGraves;
        public int arriveRadius = 3;
    }
}
