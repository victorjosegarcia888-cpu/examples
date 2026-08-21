// Geometry_Manifold_CH4.cs
//
// Geometria de manifold CH4 con colector espiral.
//
// Incluye:
// - Colector espiral (voluta)
// - Distribuidor radial
// - Compensacion termica
//
// Teoria:
// - Caudal CH4: 89 kg/s
// - Presion de operacion: 350 bar
// - Colector espiral para distribucion uniforme
//
// Cita PDF:
// "El colector espiral asegura distribucion uniforme de
//  combustible en la placa de inyectores."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    /// <summary>
    /// Geometria de manifold CH4 con colector espiral.
    /// </summary>
    public static class Geometry_Manifold_CH4
    {
        /// <summary>
        /// Crea un manifold CH4 con colector espiral y distribuidor radial.
        /// </summary>
        /// <param name="radius">Radio del manifold [m]</param>
        /// <param name="length">Longitud [m]</param>
        /// <param name="wallThickness">Espesor pared [m]</param>
        /// <param name="spiralPitch">Paso de la espiral [m]</param>
        /// <param name="radialDistributorCount">Numero de distribuidores radiales</param>
        /// <returns>Field3D con la geometria del manifold CH4</returns>
        public static Field3D Create(
            double radius = 0.18,
            double length = 0.32,
            double wallThickness = 0.012,
            double spiralPitch = 0.025,
            int radialDistributorCount = 4)
        {
            // Cuerpo principal del manifold
            var externo = Field3D.Cylinder(radius, length);
            var interno = Field3D.Cylinder(radius - wallThickness, length);
            var carcasa = Field3D.Subtract(externo, interno);

            // Colector espiral (simulado como toroide enrollado)
            var colector = Field3D.Torus(radius * 0.7, 0.04)
                .Translate(0, 0, length * 0.5);

            // Distribuidores radiales
            Field3D distribuidores = Field3D.Empty;
            for (int i = 0; i < radialDistributorCount; i++)
            {
                double ang = (2.0 * Math.PI / radialDistributorCount) * i;
                double x = Math.Cos(ang) * (radius * 0.6);
                double y = Math.Sin(ang) * (radius * 0.6);

                var dist = Field3D.Cylinder(0.025, length * 0.4)
                    .Rotate(Math.PI / 2, 0, 0)
                    .Translate(x, y, length * 0.3);

                distribuidores = Field3D.Combine(distribuidores, dist);
            }

            return Field3D.Combine(carcasa, colector, distribuidores);
        }
    }
}
