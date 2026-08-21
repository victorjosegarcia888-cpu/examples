// Geometry_Nozzle_v06.cs
//
// Geometria de la tobera campana Rao FFSC v06.
// Usando PicoGK Voxels API: voxSphere con radios variables.
//
// Teoria:
// - Contorno de tobera Rao (metodo de caracteristicas)
// - Expansion ratio Ae / At
// - Optimizacion de contorno para minima perdida de momento

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Nozzle_v06
    {
        public static Voxels Create(EngineParams p)
        {
            float Rt = (float)p.ThroatRadius;
            float Re = (float)p.ExitRadius;
            float Lc = (float)p.ChamberLength;
            float Lstar = (float)p.Lstar;

            Voxels nozzle = new Voxels();

            // Contorno de tobera desde garganta hasta salida
            // Usando perfil Rao simplificado (expansion exponencial)
            int steps = 40;
            float Lnozzle = Lstar * 0.8f;

            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                float z = Lc + Lstar * 0.3f + t * Lnozzle;
                
                // Radio evoluciona desde Rt hasta Re segun perfil Rao
                double expansion = Math.Pow(t, 0.6);
                float r = Rt + (Re - Rt) * (float)expansion;
                
                // Grosor de pared variable
                float wallThickness = 0.015f + 0.01f * t;
                
                nozzle += Voxels.voxSphere(new Vector3(0, 0, z), r);
                // Capa interna para simular pared
                nozzle += Voxels.voxSphere(new Vector3(0, 0, z), r - wallThickness);
            }

            return nozzle;
        }
    }
}
