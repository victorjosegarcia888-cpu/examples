// Geometry_Manifold_LOX.cs
//
// Geometria de manifold LOX avanzado.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    public static class Geometry_Manifold_LOX
    {
        public static Voxels Create(
            double radius = 0.18,
            double length = 0.32,
            double wallThickness = 0.012)
        {
            float R = (float)radius;
            float L = (float)length;
            float wt = (float)wallThickness;

            // Cuerpo principal: cilindro de esferas
            Voxels manifold = Voxels.voxSphere(new Vector3(0, 0, 0), R);
            for (int i = 1; i < 6; i++)
            {
                float z = i * L / 6.0f;
                manifold += Voxels.voxSphere(new Vector3(0, 0, z), R);
            }

            // Colector toroidal
            float Rtor = R * 0.8f;
            for (int i = 0; i < 12; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 12.0f;
                float x = (float)Math.Cos(ang) * Rtor;
                float y = (float)Math.Sin(ang) * Rtor;
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.3f), 0.04f);
            }

            // Bifurcaciones en Y
            float bx = R * 0.6f;
            manifold += Voxels.voxSphere(new Vector3(bx, 0, L * 0.6f), 0.05f);
            manifold += Voxels.voxSphere(new Vector3(-bx, 0, L * 0.6f), 0.05f);
            manifold += Voxels.voxSphere(new Vector3(bx * 0.7f, bx * 0.7f, L * 0.6f), 0.04f);
            manifold += Voxels.voxSphere(new Vector3(-bx * 0.7f, -bx * 0.7f, L * 0.6f), 0.04f);

            // Valvulas redundantes
            for (int i = 0; i < 4; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 4.0f;
                float x = (float)Math.Cos(ang) * (R + 0.05f);
                float y = (float)Math.Sin(ang) * (R + 0.05f);
                manifold += Voxels.voxSphere(new Vector3(x, y, L * 0.4f), 0.03f);
            }

            return manifold;
        }
    }
}
