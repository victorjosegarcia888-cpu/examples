// Geometry_Cooling_v06.cs
//
// Geometria de canales de refrigeracion FFSC v06.
// Usando PicoGK Voxels API: voxSphere para canales helicoidales.
//
// Teoria:
// - Refrigeracion regenerativa (regenerative cooling)
// - Numero de Nusselt: Nu = h*D/k
// - Correlacion de Bartz para flujo de calor critico
// - Film cooling: capa limite de refrigerante
// - Canales helicoidales (helical channels) para turbulencia

using PicoGK;
using System.Numerics;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Geometry
{
    public static class Geometry_Cooling_v06
    {
        public static Voxels Primary(Voxels chamber, Voxels spike, EngineParams p, double channelWidth = 0.006, double channelHeight = 0.0015)
        {
            Voxels cooling = new Voxels();

            float w = (float)channelWidth;
            float h = (float)channelHeight;
            float zStart = 0.0f;
            float zEnd = (float)p.ChamberLength;

            // Canales helicoidales primarios en la camara
            int channels = 48;
            float pitch = 0.03f;

            for (int c = 0; c < channels; c++)
            {
                float angleOffset = (float)(c * 2.0 * Math.PI / channels);
                
                for (float z = zStart; z <= zEnd; z += h * 0.5f)
                {
                    float angle = angleOffset + z / pitch;
                    float radius = 0.3f; // Radio aproximado de la camara
                    
                    float x = radius * (float)Math.Cos(angle);
                    float y = radius * (float)Math.Sin(angle);
                    
                    // Canal como esfera pequena a lo largo de helice
                    cooling += Voxels.voxSphere(new Vector3(x, y, z), w * 0.4f);
                }
            }

            return cooling;
        }

        public static Voxels Secondary(Voxels chamber, Voxels spike, EngineParams p, double filmThickness = 0.002)
        {
            Voxels filmCooling = new Voxels();

            float ft = (float)filmThickness;

            // Capa de film cooling en pared interior de tobera
            int steps = 60;
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                float z = (float)(p.ChamberLength + t * p.Lstar * 0.8);
                float r = 0.25f - ft * 0.5f; // Radio interior aproximado
                
                filmCooling += Voxels.voxSphere(new Vector3(0, 0, z), r);
                filmCooling += Voxels.voxSphere(new Vector3(0, 0, z), r + ft);
            }

            return filmCooling;
        }
    }
}
