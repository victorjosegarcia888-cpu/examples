// Geometry_Supports.cs
//
// Geometria de soportes del motor FFSC.
//
// Incluye:
// - Montantes (struts)
// - Placa base
// - Estructura de sujecion
//
// Teoria:
// - 4 montantes para distribucion de carga
// - Angulo optimo: 45 grados respecto a vertical
// - Material composite o titanio

using PicoGK;

namespace FFSC_PicoGK.Geometry.Supports
{
    /// <summary>
    /// Geometria de soportes del motor FFSC.
    /// </summary>
    public static class Geometry_Supports
    {
        /// <summary>
        /// Crea la estructura de soportes completa.
        /// </summary>
        /// <param name="strutCount">Numero de montantes</param>
        /// <param name="strutRadius">Radio de montantes [m]</param>
        /// <param name="strutLength">Longitud de montantes [m]</param>
        /// <param name="basePlateRadius">Radio de placa base [m]</param>
        /// <returns>Field3D con la geometria de soportes</returns>
        public static Field3D Create(
            int strutCount = 4,
            double strutRadius = 0.03,
            double strutLength = 0.40,
            double basePlateRadius = 0.50)
        {
            Field3D struts = Field3D.Empty;

            for (int i = 0; i < strutCount; i++)
            {
                double ang = (2.0 * Math.PI / strutCount) * i;
                double x = Math.Cos(ang) * basePlateRadius * 0.6;
                double y = Math.Sin(ang) * basePlateRadius * 0.6;

                var strut = Field3D.Cylinder(strutRadius, strutLength)
                    .Rotate(Math.PI / 4, 0, 0)
                    .Translate(x, y, -strutLength * 0.7);

                struts = Field3D.Combine(struts, strut);
            }

            // Placa base
            var basePlate = Field3D.Cylinder(basePlateRadius, 0.04)
                .Translate(0, 0, -strutLength);

            return Field3D.Combine(struts, basePlate);
        }
    }
}
