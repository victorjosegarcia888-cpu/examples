// Geometry_Manifold_FFSC.cs
//
// Geometria del manifold FFSC completo.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    public static class Geometry_Manifold_FFSC
    {
        public static Voxels Create(
            double radius = 0.20,
            double length = 0.40,
            double wallThickness = 0.012,
            int branchCount = 6,
            double preburnerLineRadius = 0.05,
            double returnLineRadius = 0.04)
        {
            float R = (float)radius;
            float L = (float)length;

            // Cuerpo principal
            Voxels manifold = Voxels.voxSphere(new Vector3(0, 0, 0), R);
            for (int i = 1; i < 7; i++)
            {
                float z = i * L / 7.0f;
                manifold += Voxels.voxSphere(new Vector3(0, 0, z), R);
            }

            // Ramas principales
            for (int i = 0; i < branchCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / branchCount;
                float x = (float)Math.Cos(ang) * R;
                float y = (float)Math.Sin(ang) * R;
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.5f), 0.05f);
                manifold += Voxels.voxSphere(new Vector3(x * 1.3f, y * 1.3f, L * 0.7f), 0.04f);
            }

            // Linea de preburner
            float pbX = R * 0.5f;
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.2f), (float)preburnerLineRadius);
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.5f), (float)preburnerLineRadius);
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.8f), (float)preburnerLineRadius);

            // Linea de retorno
            float retX = -R * 0.5f;
            manifold += Voxels.voxSphere(new Vector3(retX, 0, -L * 0.1f), (float)returnLineRadius);
            manifold += Voxels.voxSphere(new Vector3(retX, 0, -L * 0.4f), (float)returnLineRadius);

            // Linea de mezcla
            manifold += Voxels.voxSphere(new Vector3(0, R * 0.6f, L * 0.2f), 0.03f);
            manifold += Voxels.voxSphere(new Vector3(0, R * 0.9f, L * 0.3f), 0.025f);

            return manifold;
        }
    }
}
