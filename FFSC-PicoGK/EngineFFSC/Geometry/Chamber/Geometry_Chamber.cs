// Geometry_Chamber.cs
//
// Geometria completa de la camara de combustion FFSC.
// Usando PicoGK Voxels API: voxSphere, operadores + - &
//
// Teoria:
// - L* = Vc / At (longitud caracteristica)
// - Vc = volumen camara
// - Ac = At * CR (ratio de contraccion)
// - De Laval nozzle: Mach = 1 en garganta

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Geometry.Chamber
{
    public static class Geometry_Chamber
    {
        public static Voxels Create(EngineParams p)
        {
            float Rc = (float)p.ChamberRadius;
            float Lc = (float)p.ChamberLength;
            float Rt = (float)p.ThroatRadius;
            float Lstar = (float)p.Lstar;
            float CR = (float)p.ContractionRatio;
            Voxels chamber = Voxels.voxSphere(new Vector3(0, 0, Lc * 0.5f), Rc);
            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc * 0.75f), Rc);
            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc), Rc);
            chamber += Voxels.voxSphere(new Vector3(0, 0, Lc * 1.25f), Rc * 0.7f);

            // Seccion convergente
            for (int i = 0; i < 8; i++)
            {
                float t = i / 8.0f;
                float r = Rc * (1.0f - t * 0.5f);
                float z = Lc + t * Lc * 0.5f;
                chamber += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            return chamber;
        }

        public static Voxels Create(
            double chamberRadius = 0.35,
            double chamberLength = 0.50,
            double throatRadius = 0.12,
            double lstar = 1.2,
            double contractionRatio = 6.0)
        {
            var p = new EngineParams
            {
                ChamberRadius = chamberRadius,
                ChamberLength = chamberLength,
                ThroatRadius = throatRadius,
                Lstar = lstar,
                ContractionRatio = contractionRatio
            };
            return Create(p);
        }
    }
}
