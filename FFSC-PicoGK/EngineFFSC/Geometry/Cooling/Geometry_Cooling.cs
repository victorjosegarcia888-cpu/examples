// Geometry_Cooling.cs
//
// Geometria de canales de refrigeracion regenerativa.
//
// Incluye:
// - Canales helicoidales primarios
// - Canales helicoidales secundarios
// - Canales en manifold
//
// Teoria:
// - Refrigeracion regenerativa: CH4 fluye por canales en pared
// - Paso optimo: 2-3 veces ancho de canal
// - Numero de Nusselt para conveccion interna
//
// Cita PDF:
// "Los canales de refrigeracion regenerativa permiten temperaturas
//  de pared de hasta 1200 K en camara LOX/CH4."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Cooling
{
    /// <summary>
    /// Geometria de canales de refrigeracion regenerativa.
    /// </summary>
    public static class Geometry_Cooling
    {
        /// <summary>
        /// Crea canales de refrigeracion primarios alrededor de la camara.
        /// </summary>
        /// <param name="chamber">Geometria de la camara</param>
        /// <param name="spike">Geometria del spike</param>
        /// <param name="channelRadius">Radio del canal [m]</param>
        /// <param name="pitch">Paso de la helice [m]</param>
        /// <returns>Field3D con los canales de refrigeracion</returns>
        public static Field3D Primary(Field3D chamber, Field3D spike,
            double channelRadius = 0.006, double pitch = 0.02)
        {
            Field3D canales = Field3D.Empty;
            double altura = 0.45;

            for (double z = 0; z < altura; z += pitch)
            {
                double ang = z * 10.0;
                double x = Math.Cos(ang) * 0.22;
                double y = Math.Sin(ang) * 0.22;

                var corte = Field3D.Cylinder(channelRadius, pitch * 0.8)
                    .Translate(x, y, z);
                canales = Field3D.Combine(canales, corte);
            }

            return Field3D.Subtract(chamber, canales);
        }

        /// <summary>
        /// Crea canales de refrigeracion secundarios.
        /// </summary>
        public static Field3D Secondary(Field3D chamber, Field3D spike,
            double channelRadius = 0.004, double pitch = 0.015)
        {
            Field3D canales = Field3D.Empty;
            double altura = 0.45;

            for (double z = 0; z < altura; z += pitch)
            {
                double ang = z * 14.0;
                double x = Math.Cos(ang) * 0.18;
                double y = Math.Sin(ang) * 0.18;

                var corte = Field3D.Cylinder(channelRadius, pitch * 0.8)
                    .Translate(x, y, z);
                canales = Field3D.Combine(canales, corte);
            }

            return Field3D.Subtract(chamber, canales);
        }

        /// <summary>
        /// Crea canales de refrigeracion para el manifold.
        /// </summary>
        public static Field3D Manifold(Field3D manifold,
            double channelRadius = 0.008, double pitch = 0.02,
            double trayectoriaRadius = 0.12, double length = 0.32)
        {
            Field3D canales = Field3D.Empty;

            for (double z = 0; z < length; z += pitch)
            {
                double ang = z * 10.0;
                double x = Math.Cos(ang) * trayectoriaRadius;
                double y = Math.Sin(ang) * trayectoriaRadius;

                var corte = Field3D.Cylinder(channelRadius, pitch * 0.8)
                    .Translate(x, y, z);
                canales = Field3D.Combine(canales, corte);
            }

            return Field3D.Subtract(manifold, canales);
        }
    }
}
