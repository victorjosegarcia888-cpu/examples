// Geometry_Chamber_v06.cs
//
// Geometria de la camara de combustion FFSC v06.
// Usando PicoGK Voxels API: voxSphere, operador + para union.
//
// Teoria:
// - L* = Vc / At (longitud caracteristica)
// - CR = Ac / At (ratio de contraccion)
// - Tobera De Laval: Mach = 1 en garganta
// - Inestabilidad de combustion: ondas acusticas en camara

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Chamber_v06
    {
        public static Voxels Create(EngineParams p)
        {
            float Rc = (float)p.ChamberRadius;
            float Lc = (float)p.ChamberLength;
            float Rt = (float)p.ThroatRadius;
            float CR = (float)p.ContractionRatio;
            float Lstar = (float)p.Lstar;

            Voxels chamber = new Voxels();

            // Cuerpo cilindrico de la camara (esferas apiladas)
            for (float z = 0.0f; z <= Lc; z += Lc * 0.15f)
            {
                chamber += Voxels.voxSphere(new Vector3(0, 0, z), Rc);
            }

            // Zona de mezclado FFSC (convergencia suave hacia garganta)
            float mixLength = Lc * 0.25f;
            for (int i = 0; i < 6; i++)
            {
                float t = i / 6.0f;
                float r = Rc * (1.0f - t * (1.0f - Rt / Rc) * 0.5f);
                float z = Lc + t * mixLength;
                chamber += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            // Seccion convergente (De Laval hasta garganta)
            for (int i = 0; i < 10; i++)
            {
                float t = i / 10.0f;
                float r = Rc * (1.0f - t * 0.65f);
                float z = Lc + mixLength + t * (Rt / Rc) * 0.3f;
                chamber += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            return chamber;
        }

        public static Voxels Create()
        {
            var p = new EngineParams();
            return Create(p);
        }
    }
}
