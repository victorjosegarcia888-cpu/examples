// Geometry_Manifold_CH4.cs
//
// Geometria de manifold CH4 con colector espiral.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    public static class Geometry_Manifold_CH4
    {
        public static Voxels Create(
            double radius = 0.18,
            double length = 0.32,
            double wallThickness = 0.012,
            double spiralPitch = 0.025,
            int radialDistributorCount = 4)
        {
            float R = (float)radius;
            float L = (float)length;

            // Cuerpo principal
            Voxels manifold = Voxels.voxSphere(new Vector3(0, 0, 0), R);
            for (int i = 1; i < 6; i++)
            {
                float z = i * L / 6.0f;
                manifold += Voxels.voxSphere(new Vector3(0, 0, z), R);
            }

            // Colector espiral
            float Rcol = R * 0.7f;
            for (int i = 0; i < 12; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 12.0f;
                float x = (float)Math.Cos(ang) * Rcol;
                float y = (float)Math.Sin(ang) * Rcol;
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.5f), 0.04f);
            }

            // Distribuidores radiales
            for (int i = 0; i < radialDistributorCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / radialDistributorCount;
                float x = (float)Math.Cos(ang) * (R * 0.6f);
                float y = (float)Math.Sin(ang) * (R * 0.6f);
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.3f), 0.025f);
                manifold += Voxels.voxSphere(new Vector3(x * 1.2f, y * 1.2f, L * 0.5f), 0.025f);
            }

            return manifold;
        }
    }
}
