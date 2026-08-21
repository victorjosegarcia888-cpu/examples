// Geometry_Supports.cs
//
// Geometria de soportes del motor FFSC.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Supports
{
    public static class Geometry_Supports
    {
        public static Voxels Create(
            int strutCount = 4,
            double strutRadius = 0.03,
            double strutLength = 0.40,
            double basePlateRadius = 0.50)
        {
            float sr = (float)strutRadius;
            float sl = (float)strutLength;
            float bpr = (float)basePlateRadius;

            Voxels supports = new Voxels();

            // Struts at 45 degree angles
            for (int i = 0; i < strutCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / strutCount;
                float bx = (float)Math.Cos(ang) * bpr * 0.6f;
                float by = (float)Math.Sin(ang) * bpr * 0.6f;

                for (float t = 0; t <= 1.0f; t += 0.1f)
                {
                    float x = bx * (1.0f - t * 0.5f);
                    float y = by * (1.0f - t * 0.5f);
                    float z = -t * sl;
                    supports += Voxels.voxSphere(new Vector3(x, y, z), sr);
                }
            }

            // Base plate
            for (float z = -sl; z <= -sl + 0.04f; z += 0.02f)
            {
                supports += Voxels.voxSphere(new Vector3(0, 0, z), bpr);
            }

            return supports;
        }
    }
}
