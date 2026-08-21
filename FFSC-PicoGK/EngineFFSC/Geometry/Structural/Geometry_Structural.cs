// Geometry_Structural.cs
//
// Geometria estructural del motor FFSC.
//
// Incluye:
// - Marco de empuje (thrust frame)
// - Montajes de gimbal
// - Faldon (skirt)
//
// Teoria:
// - Carga de empuje: 2.5 MN (Raptor 3 class)
// - Factor de seguridad estructural: 1.5
// - Material: Inconel 718, Acero maraging
//
// Cita PDF:
// "El thrust frame transfiere la carga de empuje a la estructura
//  del vehiculo. Los montajes de gimbal permiten vectorizacion."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Structural
{
    /// <summary>
    /// Geometria estructural del motor FFSC.
    /// </summary>
    public static class Geometry_Structural
    {
        /// <summary>
        /// Crea el marco de empuje completo.
        /// </summary>
        /// <param name="frameWidth">Ancho del marco [m]</param>
        /// <param name="gimbalRadius">Radio de montaje gimbal [m]</param>
        /// <param name="skirtHeight">Altura del faldon [m]</param>
        /// <param name="skirtRadius">Radio del faldon [m]</param>
        /// <returns>Field3D con la geometria estructural</returns>
        public static Field3D Create(
            double frameWidth = 0.08,
            double gimbalRadius = 0.10,
            double skirtHeight = 0.20,
            double skirtRadius = 0.40)
        {
            // Thrust frame (cuadro estructural)
            var frameVertical1 = Field3D.Cylinder(frameWidth * 0.5, skirtHeight)
                .Translate(skirtRadius * 0.4, 0, -skirtHeight * 0.5);
            var frameVertical2 = Field3D.Cylinder(frameWidth * 0.5, skirtHeight)
                .Translate(-skirtRadius * 0.4, 0, -skirtHeight * 0.5);
            var frameHorizontal = Field3D.Cylinder(skirtRadius * 0.8, frameWidth * 0.5)
                .Rotate(Math.PI / 2, 0, 0)
                .Translate(0, 0, -skirtHeight);

            var frame = Field3D.Combine(frameVertical1, frameVertical2, frameHorizontal);

            // Montajes de gimbal
            Field3D gimbals = Field3D.Empty;
            for (int i = 0; i < 2; i++)
            {
                double x = (i == 0 ? 1 : -1) * skirtRadius * 0.3;
                var gimbal = Field3D.Cylinder(gimbalRadius * 0.3, 0.06)
                    .Translate(x, 0, -skirtHeight * 0.9);
                gimbals = Field3D.Combine(gimbals, gimbal);
            }

            // Faldon
            var skirt = Field3D.Cylinder(skirtRadius, skirtHeight)
                .Translate(0, 0, -skirtHeight);

            return Field3D.Combine(frame, gimbals, skirt);
        }
    }
}
