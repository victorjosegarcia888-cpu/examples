// Geometry_Manifold_FFSC.cs
//
// Geometria del manifold FFSC completo.
//
// Incluye:
// - Lineas de preburner
// - Lineas de retorno
// - Lineas de mezcla
// - Conexiones de turbobomba
//
// Teoria:
// - Ciclo cerrado FFSC: oxidizante y combustible pasan por preburner
// - Flujo de retorno envia gases precombustidos a turbina
// - Relacion de mezcla variable (MR = 3.6 tipico)
//
// Cita PDF:
// "El ciclo FFSC ofrece mayor eficiencia especifica que el
//  ciclo de combustion escalonada tradicional."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Manifolds
{
    /// <summary>
    /// Geometria completa del manifold FFSC.
    /// </summary>
    public static class Geometry_Manifold_FFSC
    {
        /// <summary>
        /// Crea el manifold FFSC completo con todas sus lineas.
        /// </summary>
        /// <param name="radius">Radio del manifold principal [m]</param>
        /// <param name="length">Longitud [m]</param>
        /// <param name="wallThickness">Espesor pared [m]</param>
        /// <param name="branchCount">Numero de ramas</param>
        /// <param name="preburnerLineRadius">Radio linea preburner [m]</param>
        /// <param name="returnLineRadius">Radio linea retorno [m]</param>
        /// <returns>Field3D con la geometria del manifold FFSC</returns>
        public static Field3D Create(
            double radius = 0.20,
            double length = 0.40,
            double wallThickness = 0.012,
            int branchCount = 6,
            double preburnerLineRadius = 0.05,
            double returnLineRadius = 0.04)
        {
            // Cuerpo principal del manifold
            var externo = Field3D.Cylinder(radius, length);
            var interno = Field3D.Cylinder(radius - wallThickness, length);
            var carcasa = Field3D.Subtract(externo, interno);

            // Ramas principales
            Field3D ramas = Field3D.Empty;
            for (int i = 0; i < branchCount; i++)
            {
                double ang = (2.0 * Math.PI / branchCount) * i;
                double x = Math.Cos(ang) * radius;
                double y = Math.Sin(ang) * radius;

                var rama = Field3D.Cylinder(0.05, 0.22)
                    .Rotate(Math.PI / 2, 0, 0)
                    .Translate(x, y, length * 0.5);
                ramas = Field3D.Combine(ramas, rama);
            }

            // Linea de preburner (cilindro vertical)
            var preburner = Field3D.Cylinder(preburnerLineRadius, length * 0.8)
                .Translate(radius * 0.5, 0, -length * 0.2);

            // Linea de retorno (curva)
            var retorno = Field3D.Cylinder(returnLineRadius, length * 0.6)
                .Translate(-radius * 0.5, 0, -length * 0.1);

            // Linea de mezcla
            var mezcla = Field3D.Cylinder(0.03, length * 0.5)
                .Translate(0, radius * 0.6, length * 0.2);

            return Field3D.Combine(carcasa, ramas, preburner, retorno, mezcla);
        }
    }
}
