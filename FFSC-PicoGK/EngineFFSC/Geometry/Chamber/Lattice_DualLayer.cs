// Lattice_DualLayer.cs
//
// Lattice estructural de doble capa basado en campo de tensiones.
//
// Teoria:
// - Capa 1: estructura gruesa para cargas principales
// - Capa 2: estructura fina para disipacion y vibracion
// - Interpolacion exponencial alpha(s) = 1 - exp(-k*s)
//
// Cita PDF:
// "La interpolacion exponencial permite que pequenas variaciones
//  produzcan grandes cambios en la microtopologia."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Chamber
{
    /// <summary>
    /// Lattice de doble capa.
    /// </summary>
    public static class Lattice_DualLayer
    {
        /// <summary>
        /// Genera lattice dual-layer basado en campo de tensiones.
        /// </summary>
        public static Field3D Generate(
            Field3D stressField,
            double umbralGrueso = 0.6,
            double umbralFino = 0.3,
            double radioGrueso = 0.015,
            double radioFino = 0.008)
        {
            Field3D latticeGrueso = Field3D.Empty;
            Field3D latticeFino = Field3D.Empty;

            stressField.ForEachVoxel((x, y, z, valor) =>
            {
                if (valor > umbralGrueso)
                {
                    var nodo = Field3D.Sphere(radioGrueso)
                        .Translate(x, y, z);
                    latticeGrueso = Field3D.Combine(latticeGrueso, nodo);
                }

                if (valor > umbralFino && valor <= umbralGrueso)
                {
                    var nodo = Field3D.Sphere(radioFino)
                        .Translate(x, y, z);
                    latticeFino = Field3D.Combine(latticeFino, nodo);
                }
            });

            return Field3D.Combine(latticeGrueso, latticeFino);
        }
    }
}
