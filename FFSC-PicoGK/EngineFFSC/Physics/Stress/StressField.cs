// StressField.cs
//
// Campo de tensiones volumetrico para el motor FFSC.
//
// Usado para:
// - Lattice adaptativo
// - Refuerzos estructurales
// - Optimizacion de masa
// - Cooling inteligente
//
// Cita PDF:
// "El campo de tensiones volumetrico permite optimizar
//  la distribucion de material segun la carga real."

using PicoGK;

namespace FFSC_PicoGK.Physics.Stress
{
    /// <summary>
    /// Generador de campos de tensiones.
    /// </summary>
    public static class StressField
    {
        /// <summary>
        /// Campo estatico basado en proximidad a zonas calientes.
        /// </summary>
        public static Field3D Static(Field3D camara, Field3D spike, Field3D manifold)
        {
            var stress = Field3D.Empty;
            var combinado = Field3D.Combine(camara, spike, manifold);

            combinado.ForEachVoxel((x, y, z, valor) =>
            {
                double distCentro = Math.Sqrt(x * x + y * y);
                double tension = 1.0 - distCentro;

                if (manifold.Sample(x, y, z) > 0.5)
                    tension += 0.4;
                if (spike.Sample(x, y, z) > 0.5)
                    tension += 0.3;

                tension = Math.Clamp(tension, 0.0, 1.0);

                if (tension > 0.1)
                {
                    var voxel = Field3D.Sphere(tension * 0.01)
                        .Translate(x, y, z);
                    stress = Field3D.Combine(stress, voxel);
                }
            });

            return stress;
        }

        /// <summary>
        /// Campo dinamico con oscilaciones.
        /// </summary>
        public static Field3D Dynamic(Field3D camara, Field3D spike, Field3D manifold)
        {
            var stress = Field3D.Empty;
            var combinado = Field3D.Combine(camara, spike, manifold);

            combinado.ForEachVoxel((x, y, z, valor) =>
            {
                double distCentro = Math.Sqrt(x * x + y * y);
                double oscilacion = Math.Sin(z * 12.0) * 0.2;
                double tension = (1.0 - distCentro) + oscilacion;

                if (manifold.Sample(x, y, z) > 0.5)
                    tension += 0.5;
                if (spike.Sample(x, y, z) > 0.5)
                    tension += 0.4;

                tension = Math.Clamp(tension, 0.0, 1.0);

                if (tension > 0.1)
                {
                    var voxel = Field3D.Sphere(tension * 0.012)
                        .Translate(x, y, z);
                    stress = Field3D.Combine(stress, voxel);
                }
            });

            return stress;
        }
    }
}
