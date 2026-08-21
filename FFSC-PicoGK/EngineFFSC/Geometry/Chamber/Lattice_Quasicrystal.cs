// Lattice_Quasicrystal.cs
//
// Lattice cuasicristalino basado en campo de tensiones.
//
// Teoria:
// - Patron no periodico tipo Penrose
// - Genera refuerzo estructural en zonas de alta tension
// - Aproximado mediante funciones trigonometricas
//
// Cita PDF:
// "Los patrones cuasicristalinos ofrecen propiedades mecanicas
//  superiores a las estructuras cristalinas tradicionales."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Chamber
{
    /// <summary>
    /// Lattice cuasicristalino.
    /// </summary>
    public static class Lattice_Quasicrystal
    {
        /// <summary>
        /// Genera lattice cuasicristalino basado en campo de tensiones.
        /// </summary>
        public static Field3D Generate(
            Field3D stressField,
            double escala = 0.3,
            double intensidad = 0.5)
        {
            Field3D lattice = Field3D.Empty;

            stressField.ForEachVoxel((x, y, z, valor) =>
            {
                if (valor < intensidad)
                    return;

                double qx = Math.Cos(x * escala) + Math.Cos(y * escala * 1.618);
                double qy = Math.Sin(y * escala) + Math.Sin(z * escala * 1.618);
                double magnitud = Math.Abs(qx + qy);

                if (magnitud > 1.2)
                {
                    var nodo = Field3D.Sphere(0.006)
                        .Translate(x, y, z);
                    lattice = Field3D.Combine(lattice, nodo);
                }
            });

            return lattice;
        }
    }
}
