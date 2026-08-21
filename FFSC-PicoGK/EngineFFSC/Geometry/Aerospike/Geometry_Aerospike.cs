// Geometry_Aerospike.cs
//
// Geometria de aerospike lineal.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Aerospike
{
    public static class Geometry_Aerospike
    {
        public static Voxels Create(double length = 0.55, double baseRadius = 0.15)
        {
            float L = (float)length;
            float Rbase = (float)baseRadius;

            // Spike conico truncado (series de esferas)
            Voxels spike = Voxels.voxSphere(new Vector3(0, 0, -L * 0.5f), 0.02f);
            spike += Voxels.voxSphere(new Vector3(0, 0, -L * 0.3f), 0.05f);
            spike += Voxels.voxSphere(new Vector3(0, 0, -L * 0.1f), 0.08f);
            spike += Voxels.voxSphere(new Vector3(0, 0, L * 0.1f), 0.12f);
            spike += Voxels.voxSphere(new Vector3(0, 0, L * 0.3f), 0.15f);

            // Base toroidal (serie de esferas en circulo)
            Voxels torus = Voxels.voxSphere(new Vector3(Rbase, 0, L * 0.3f), 0.03f);
            for (int i = 1; i < 16; i++)
            {
                float ang = i * 2.0f * (float)Math.PI / 16.0f;
                float x = (float)Math.Cos(ang) * Rbase;
                float y = (float)Math.Sin(ang) * Rbase;
                torus += Voxels.voxSphere(new Vector3(x, y, L * 0.3f), 0.03f);
            }

            return spike + torus;
        }

        public static Voxels Create(FFSC_PicoGK.Models.EngineParams p)
        {
            return Create(p.ChamberLength * 1.1, p.ExitRadius * 0.2);
        }
    }
}
