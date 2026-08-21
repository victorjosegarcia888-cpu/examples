// Geometry_Manifold_Fractal_CH4_v07.cs
//
// Fractal tree manifold for CH4 with recursive bifurcations.
// Theory: Murray's law scaling for cryogenic fuel distribution.
// Using PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Manifold_Fractal_CH4_v07
    {
        public static Voxels Build(
            double trunkRadius = 0.18,
            double length = 0.32,
            int depth = 3,
            double bifurcationAngle = 0.785)
        {
            float R = (float)trunkRadius;
            Voxels manifold = new Voxels();

            // Main trunk
            for (int i = 0; i < 6; i++)
            {
                float z = i * (float)length / 6.0f;
                manifold += Voxels.voxSphere(new Vector3(0, 0, z), R);
            }

            // Recursive fractal branches
            manifold = FractalBranch(manifold, new Vector3(0, 0, (float)length * 0.5f), R, depth, bifurcationAngle);

            return manifold;
        }

        private static Voxels FractalBranch(Voxels parent, Vector3 origin, float radius, int depth, double angle)
        {
            if (depth <= 0) return parent;

            float childRadius = radius * 0.707f;
            float childDist = radius * 2.0f;

            for (int i = 0; i < 2; i++)
            {
                float ang = (float)(angle * i + depth * 0.6);
                float x = origin.X + (float)Math.Cos(ang) * childDist;
                float y = origin.Y + (float)Math.Sin(ang) * childDist;
                float z = origin.Z;

                Voxels child = Voxels.voxSphere(new Vector3(x, y, z), childRadius);
                parent += child;

                parent = FractalBranch(parent, new Vector3(x, y, z), childRadius, depth - 1, angle);
            }

            return parent;
        }
    }
}
