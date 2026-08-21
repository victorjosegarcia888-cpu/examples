// Geometry_Nozzle.cs
//
// Perfil de tobera Rao-optimizado.
// Usando PicoGK Voxels API: voxSphere, operadores + - &

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Geometry.Nozzle
{
    public static class Geometry_Nozzle
    {
        public static Voxels Create(EngineParams p)
        {
            float Rt = (float)p.ThroatRadius;
            float Re = (float)p.ExitRadius;
            float Lstar = (float)p.Lstar;
            float Lnozzle = Lstar * 0.6f;
            float voxSize = 0.5f;

            Voxels nozzle = Voxels.voxSphere(new Vector3(0, 0, 0), Rt);

            // Perfil expansor divergente
            int N = 40;
            for (int i = 1; i <= N; i++)
            {
                float t = i / (float)N;
                float z = t * Lnozzle;
                float r = Rt * (1.0f + (Re / Rt - 1.0f) * (float)Math.Pow(t, 1.5f));
                if (r < Rt) r = Rt;

                nozzle += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            return nozzle;
        }

        public static Voxels Create(
            double throatRadius = 0.12,
            double exitRadius = 0.80,
            double exitAngle_deg = 14.0)
        {
            var p = new EngineParams
            {
                ThroatRadius = throatRadius,
                ExitRadius = exitRadius
            };
            return Create(p);
        }
    }
}
