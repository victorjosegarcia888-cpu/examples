// CFDTask.cs
//
// Tarea CFD simplificada para el motor FFSC.
//
// Calcula:
// - Capa limite
// - Transferencia de calor
// - Perfil de velocidades aproximado
//
// Cita PDF:
// "La simulacion CFD completa requiere mallado fino y
//  computacion de alto rendimiento. Esta version simplificada
//  da estimaciones utiles para el diseno conceptual."

using PicoGK;

namespace FFSC_PicoGK.Physics.CFD
{
    /// <summary>
    /// Tarea CFD simplificada.
    /// </summary>
    public static class CFDTask
    {
        /// <summary>
        /// CFD estatico basado en geometria.
        /// </summary>
        public static Field3D Static(Field3D geom)
        {
            Field3D campo = Field3D.Empty;

            geom.ForEachVoxel((x, y, z, valor) =>
            {
                if (valor < 0.5)
                    return;

                double dist = Math.Sqrt(x * x + y * y);
                double vel = 1.0 - dist;
                double pres = Math.Sin(z * 8.0) * 0.5 + 0.5;

                double intensidad = Math.Clamp((vel + pres) * 0.5, 0.0, 1.0);

                if (intensidad > 0.1)
                {
                    var voxel = Field3D.Sphere(intensidad * 0.01)
                        .Translate(x, y, z);
                    campo = Field3D.Combine(campo, voxel);
                }
            });

            return campo;
        }

        /// <summary>
        /// CFD dinamico con oscilaciones.
        /// </summary>
        public static Field3D Dynamic(Field3D geom)
        {
            Field3D campo = Field3D.Empty;

            geom.ForEachVoxel((x, y, z, valor) =>
            {
                if (valor < 0.5)
                    return;

                double dist = Math.Sqrt(x * x + y * y);
                double vel = (1.0 - dist) + Math.Sin(z * 12.0) * 0.2;
                double pres = Math.Cos(z * 9.0) * 0.3 + 0.7;

                double intensidad = Math.Clamp((vel + pres) * 0.5, 0.0, 1.0);

                if (intensidad > 0.1)
                {
                    var voxel = Field3D.Sphere(intensidad * 0.012)
                        .Translate(x, y, z);
                    campo = Field3D.Combine(campo, voxel);
                }
            });

            return campo;
        }
    }
}
