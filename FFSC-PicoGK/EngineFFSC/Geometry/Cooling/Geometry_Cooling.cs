// Geometry_Cooling.cs
//
// Geometria de canales de refrigeracion regenerativa.
// Usando PicoGK Voxels API.

using PicoGK;
using System.Numerics;

namespace FFSC_PicoGK.Geometry.Cooling
{
    public static class Geometry_Cooling
    {
        public static Voxels Primary(Voxels? chamber, Voxels? spike,
            double channelRadius = 0.006, double pitch = 0.02)
        {
            float r = (float)channelRadius;
            float h = (float)pitch;

            Voxels canales = new Voxels();
            double altura = 0.45;

            for (double z = 0; z < altura; z += pitch)
            {
                double ang = z * 10.0;
                double x = Math.Cos(ang) * 0.22;
                double y = Math.Sin(ang) * 0.22;

                canales += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)z), r);
                canales += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)(z + h * 0.5f)), r);
            }

            // Canales se muestran como volumen positivo en la geometria
            return canales;
        }

        public static Voxels Secondary(Voxels chamber, Voxels spike,
            double channelRadius = 0.004, double pitch = 0.015)
        {
            float r = (float)channelRadius;
            float h = (float)pitch;

            Voxels canales = new Voxels();
            double altura = 0.45;

            for (double z = 0; z < altura; z += pitch)
            {
                double ang = z * 14.0;
                double x = Math.Cos(ang) * 0.18;
                double y = Math.Sin(ang) * 0.18;

                canales += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)z), r);
                canales += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)(z + h * 0.5f)), r);
            }

            return canales;
        }

        public static Voxels Manifold(Voxels manifold,
            double channelRadius = 0.008, double pitch = 0.02,
            double trayectoriaRadius = 0.12, double length = 0.32)
        {
            float r = (float)channelRadius;
            float tr = (float)trayectoriaRadius;

            Voxels canales = new Voxels();

            for (double z = 0; z < length; z += pitch)
            {
                double ang = z * 10.0;
                double x = Math.Cos(ang) * tr;
                double y = Math.Sin(ang) * tr;

                canales += Voxels.voxSphere(new Vector3((float)x, (float)y, (float)z), r);
            }

            return canales;
        }
    }
}
