// Geometry_Manifold_Fractal_FFSC_v07.cs
//
// FFSC fractal recirculation manifold with return lines.
// Theory: Recirculation loop maintains propellant mixing and ullage control.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Manifold_Fractal_FFSC_v07
    {
        public static Voxels Build(
            double radius = 0.20,
            double length = 0.40,
            int branchCount = 6,
            double preburnerLineRadius = 0.05,
            double returnLineRadius = 0.04)
        {
            float R = (float)radius;
            float L = (float)length;

            Voxels manifold = new Voxels();

            // Main body
            for (int i = 0; i < 7; i++)
            {
                float z = i * L / 7.0f;
                manifold += Voxels.voxSphere(new Vector3(0, 0, z), R);
            }

            // Primary branches
            for (int i = 0; i < branchCount; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / branchCount;
                float x = (float)Math.Cos(ang) * R;
                float y = (float)Math.Sin(ang) * R;
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.5f), 0.05f);
                manifold += Voxels.voxSphere(new Vector3(x * 1.3f, y * 1.3f, L * 0.7f), 0.04f);
            }

            // Preburner line
            float pbX = R * 0.5f;
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.2f), (float)preburnerLineRadius);
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.5f), (float)preburnerLineRadius);
            manifold += Voxels.voxSphere(new Vector3(pbX, 0, -L * 0.8f), (float)preburnerLineRadius);

            // Return line
            float retX = -R * 0.5f;
            manifold += Voxels.voxSphere(new Vector3(retX, 0, -L * 0.1f), (float)returnLineRadius);
            manifold += Voxels.voxSphere(new Vector3(retX, 0, -L * 0.4f), (float)returnLineRadius);

            // Mixing line
            manifold += Voxels.voxSphere(new Vector3(0, R * 0.6f, L * 0.2f), 0.03f);
            manifold += Voxels.voxSphere(new Vector3(0, R * 0.9f, L * 0.3f), 0.025f);

            return manifold;
        }
    }
}
