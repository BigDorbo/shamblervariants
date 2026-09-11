using UnityEngine;
using Verse;

namespace ShamblerVariants
{
    public static class SVTether
    {
        public const float SegmentLength = 0.25f;
        public const float WaveAmplitude = 0.16f;
        public const float WaveCycles = 2.5f;
        public const int MaxSegments = 48;

        public static void Draw(Vector3 from, Vector3 to, Material edge, Material core, float width, float phase)
        {
            float dx = to.x - from.x;
            float dz = to.z - from.z;
            float span = Mathf.Sqrt(dx * dx + dz * dz);
            if (span < 0.05f)
            {
                return;
            }
            float nx = -dz / span;
            float nz = dx / span;
            int segments = Mathf.CeilToInt(span / SegmentLength);
            if (segments < 2)
            {
                segments = 2;
            }
            if (segments > MaxSegments)
            {
                segments = MaxSegments;
            }
            Vector3 prev = PointOn(from, dx, dz, nx, nz, 0f, phase);
            for (int i = 1; i <= segments; i++)
            {
                Vector3 next = PointOn(from, dx, dz, nx, nz, (float)i / segments, phase);
                GenDraw.DrawLineBetween(prev, next, edge, width);
                GenDraw.DrawLineBetween(prev, next, core, width * 0.5f);
                prev = next;
            }
        }

        private static Vector3 PointOn(Vector3 origin, float dx, float dz,
            float nx, float nz, float t, float phase)
        {
            float wave = Mathf.Sin(t * WaveCycles * 2f * Mathf.PI + phase)
                * WaveAmplitude * (1f - Mathf.Abs(2f * t - 1f));
            Vector3 p = origin;
            p.x = origin.x + dx * t + nx * wave;
            p.z = origin.z + dz * t + nz * wave;
            return p;
        }
    }
}
