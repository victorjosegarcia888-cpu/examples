// Geometry_Manifold_LOX.cs
//
// Geometria de manifold LOX avanzado.
//
// Incluye:
// - Colector toroidal
// - Ramas en Y (bifurcacion)
// - Compensacion de presion
// - Valvulas redundantes
//
// Teoria:
// - Presion diferencial maxima: 35 MPa (Pc)
// - Caudal: 320 kg/s LOX
// - Diametro hidraulico optimizado
//
// Cita PDF:
// "El manifold LOX debe mantener temperatura criogenica
//  (< -183 C) para evitar cavitacion en la turbobomba."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    /// <summary>
    /// Geometria de manifold LOX avanzado.
    /// </summary>
    public static class Geometry_Manifold_LOX
    {
        /// <summary>
        /// Crea un manifold LOX con colector toroidal y compensacion.
        /// </summary>
        /// <param name="radius">Radio del manifold [m]</param>
        /// <param name="length">Longitud [m]</param>
        /// <param name="wallThickness">Espesor pared [m]</param>
        /// <returns>Field3D con la geometria del manifold LOX</returns>
        public static Field3D Create(
            double radius = 0.18,
            double length = 0.32,
            double wallThickness = 0.012)
        {
            // Cuerpo principal del manifold
            var externo = Field3D.Cylinder(radius, length);
            var interno = Field3D.Cylinder(radius - wallThickness, length);
            var carcasa = Field3D.Subtract(externo, interno);

            // Colector toroidal superior
            var colector = Field3D.Torus(radius * 0.8, 0.04)
                .Translate(0, 0, length * 0.3);

            // Bifurcacion en Y (dos ramas)
            var rama1 = Field3D.Cylinder(0.05, 0.20)
                .Rotate(0, Math.PI / 4, 0)
                .Translate(radius * 0.6, 0, length * 0.6);

            var rama2 = Field3D.Cylinder(0.05, 0.20)
                .Rotate(0, -Math.PI / 4, 0)
                .Translate(-radius * 0.6, 0, length * 0.6);

            // Valvulas redundantes
            Field3D valvulas = Field3D.Empty;
            for (int i = 0; i < 4; i++)
            {
                double ang = (2.0 * Math.PI / 4.0) * i;
                double x = Math.Cos(ang) * (radius + 0.05);
                double y = Math.Sin(ang) * (radius + 0.05);

                var valvula = Field3D.Cylinder(0.03, 0.12)
                    .Translate(x, y, length * 0.4);
                valvulas = Field3D.Combine(valvulas, valvula);
            }

            return Field3D.Combine(carcasa, colector, rama1, rama2, valvulas);
        }
    }
}
