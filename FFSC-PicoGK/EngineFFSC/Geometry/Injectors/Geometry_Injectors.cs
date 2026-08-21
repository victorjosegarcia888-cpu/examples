// Geometry_Injectors.cs
//
// Geometria de inyectores coaxiales FFSC.
//
// Teoria:
// - Inyector coaxial: LOX en post central, CH4 en anillo exterior
// - Atomizacion por cortadura de shear
// - Angulo de swirl para atomizacion fina
// - Numero tipico: 24-32 inyectores
// - Patron de mezcla optimo para combustion estable
//
// Cita PDF:
// "Los inyectores coaxiales son el estandar en motores de alta
//  performance como Raptor y BE-4. El flujo de LOX en nucleo
//  y CH4 en anillo permite mezcla rapida y combustion completa."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Injectors
{
    /// <summary>
    /// Geometria de placa de inyectores coaxiales FFSC.
    /// </summary>
    public static class Geometry_Injectors
    {
        /// <summary>
        /// Crea la placa de inyectores completa.
        /// </summary>
        /// <param name="count">Numero de inyectores</param>
        /// <param name="plateRadius">Radio de la placa [m]</param>
        /// <param name="loxPostRadius">Radio del post LOX [m]</param>
        /// <param name="ch4AnnulusWidth">Ancho del anillo CH4 [m]</param>
        /// <param name="injectorLength">Longitud del inyector [m]</param>
        /// <returns>Field3D con la geometria de los inyectores</returns>
        public static Field3D Create(
            int count = 32,
            double plateRadius = 0.24,
            double loxPostRadius = 0.008,
            double ch4AnnulusWidth = 0.004,
            double injectorLength = 0.06)
        {
            // Placa base
            var placa = Field3D.Cylinder(plateRadius, 0.02);

            Field3D inyectores = Field3D.Empty;

            for (int i = 0; i < count; i++)
            {
                double ang = (2.0 * Math.PI / count) * i;
                double r = plateRadius * 0.7;
                double x = Math.Cos(ang) * r;
                double y = Math.Sin(ang) * r;

                // Post LOX central
                var postLOX = Field3D.Cylinder(loxPostRadius, injectorLength)
                    .Translate(x, y, 0.01);

                // Anillo CH4
                var annulusCH4 = Field3D.Cylinder(loxPostRadius + ch4AnnulusWidth, injectorLength)
                    .Subtract(Field3D.Cylinder(loxPostRadius, injectorLength))
                    .Translate(x, y, 0.01);

                inyectores = Field3D.Combine(inyectores, postLOX, annulusCH4);
            }

            return Field3D.Combine(placa, inyectores);
        }

        /// <summary>
        /// Crea inyectores con parametros de motor.
        /// </summary>
        public static Field3D Create(FFSC_PicoGK.Models.EngineParams p)
        {
            return Create(p.MixtureRatio > 3.0 ? 32 : 24,
                p.ChamberRadius * 0.7);
        }
    }
}
