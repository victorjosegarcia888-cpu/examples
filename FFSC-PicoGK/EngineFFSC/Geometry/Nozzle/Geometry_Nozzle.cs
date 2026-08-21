// Geometry_Nozzle.cs
//
// Perfil de tobera Rao-optimizado (parabolico).
//
// Teoria:
// - Tobera convergente-divergente (De Laval)
// - Contorno de Rao (1963) para empuje optimizado
// - Ecuacion: y = sqrt(At) * f(x) donde f(x) es parabola
// - Angulo de salida tipico: 12-15 grados
//
// Cita PDF:
// "La optimizacion de Rao minimiza la perdida por divergencia
//  mientras mantiene la longitud de la tobera razonable."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Nozzle
{
    /// <summary>
    /// Geometria de tobera Rao-optimizada.
    /// </summary>
    public static class Geometry_Nozzle
    {
        /// <summary>
        /// Crea una tobera completa con perfil Rao-optimizado.
        /// </summary>
        /// <param name="p">Parametros del motor</param>
        /// <returns>Field3D con la geometria de la tobera</returns>
        public static Field3D Create(FFSC_PicoGK.Models.EngineParams p)
        {
            double Rt = p.ThroatRadius;
            double Re = p.ExitRadius;
            double At = p.At;
            double Ae = p.Ae;
            double Lstar = p.Lstar;
            double CR = p.ContractionRatio;

            // Longitud de la tobera
            double Lnozzle = Lstar * 0.6;
            double Lthroat = Lstar * 0.4; // posicion de garganta en la tobera

            // Perfil Rao-optimizado
            // Usamos discretizacion axial
            int N = 200;
            var nozzle = Field3D.Empty;

            for (int i = 0; i < N; i++)
            {
                double t = i / (double)(N - 1); // 0 a 1
                double z = t * Lnozzle;

                // Radio en funcion de z segun perfil Rao
                double r = RaoRadius(z, Rt, Re, Lthroat, Lnozzle);
                double A = Math.PI * r * r;

                // Voxelizar el anillo en esta posicion z
                var slice = Field3D.Circle(r).Translate(0, 0, z);
                nozzle = Field3D.Combine(nozzle, slice);
            }

            return nozzle;
        }

        /// <summary>
        /// Perfil Rao-optimizado para radio de tobera.
        /// </summary>
        private static double RaoRadius(
            double z,
            double Rt,
            double Re,
            double Lthroat,
            double Lnozzle)
        {
            // Posicion normalizada desde garganta
            double t = z / Lnozzle; // 0 en garganta, 1 en salida

            // Perfil parabolico tipo Rao
            // R(z) = Rt * sqrt(1 + (Ae/At - 1) * (2*t - t^2) / 0.5)
            // Simplificacion: perfil parabolico suave

            double eps = Re * Re / (Rt * Rt); // expansion ratio (Ae/At)
            double Rnozzle = Rt * Math.Sqrt(1.0 + (eps - 1.0) * Math.Pow(t, 1.5));

            // Angulo de salida optimo (~14 grados)
            double exitAngleRad = 14.0 * Math.PI / 180.0;
            if (t > 0.85)
            {
                // Redondear salida para evitar separacion de flujo
                double blend = (t - 0.85) / 0.15;
                Rnozzle = Rnozzle * (1.0 - blend) + Re * blend;
            }

            return Math.Max(Rt, Rnozzle);
        }

        /// <summary>
        /// Crea la tobera con parametros directos.
        /// </summary>
        public static Field3D Create(
            double throatRadius = 0.12,
            double exitRadius = 0.80,
            double exitAngle_deg = 14.0)
        {
            var p = new FFSC_PicoGK.Models.EngineParams
            {
                ThroatRadius = throatRadius,
                ExitRadius = exitRadius
            };
            return Create(p);
        }
    }
}
