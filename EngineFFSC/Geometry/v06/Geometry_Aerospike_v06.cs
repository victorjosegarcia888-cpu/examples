// Geometry_Aerospike_v06.cs
//
// Geometria del aerospike truncado FFSC v06.
// Usando PicoGK Voxels API: voxSphere para spike y plug base.
//
// Teoria:
// - Compensacion de altitud (altitude compensation)
// - NPR (Nozzle Pressure Ratio) adaptativo
// - Contorno de spike truncado con plug base
// - Ventaja: Isp constante sobre rango de altitudes

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Aerospike_v06
    {
        public static Voxels Create(EngineParams p)
        {
            float Rt = (float)p.ThroatRadius;
            float Re = (float)p.ExitRadius;
            float Lc = (float)p.ChamberLength;

            Voxels spike = new Voxels();
            Voxels plug = new Voxels();

            // Spike truncado (cono truncado)
            float spikeLength = Lc * 1.5f;
            float truncationRatio = 0.7f;
            float spikeTipRadius = Rt * 0.3f;
            float spikeBaseRadius = Re * 0.9f;

            int steps = 50;
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                float z = Lc * 0.2f + t * spikeLength * truncationRatio;
                
                // Perfil conico del spike
                float r = spikeTipRadius + (spikeBaseRadius - spikeTipRadius) * t;
                
                // Radio de curvatura suave en punta
                if (t < 0.1f)
                {
                    r = spikeTipRadius + (spikeBaseRadius - spikeTipRadius) * t * t;
                }
                
                spike += Voxels.voxSphere(new Vector3(0, 0, z), r);
            }

            // Plug base (cierre del extremo truncado)
            float plugRadius = spikeBaseRadius * 1.1f;
            float plugThickness = 0.02f;
            for (float z = Lc * 0.2f - plugThickness; z <= Lc * 0.2f + plugThickness; z += 0.005f)
            {
                plug += Voxels.voxSphere(new Vector3(0, 0, z), plugRadius);
            }

            return spike + plug;
        }
    }
}
