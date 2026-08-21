// Geometry_Aerospike.cs
//
// Geometria de aerospike lineal (altitude-compensating).
//
// Teoria:
// - El aerospike compensa automaticamente la presion ambiental
// - Base toroidal crea efecto de expansion adaptativa
// - Altura optima segun ratio de expansion deseado
// - No requiere campana de tobera
//
// Cita PDF:
// "El aerospike lineal ofrece compensacion de altitud natural
//  sin sistemas activos. Ideal para vehiculos reutilizables."

using PicoGK;

namespace FFSC_PicoGK.Geometry.Aerospike
{
    /// <summary>
    /// Geometria de aerospike lineal FFSC.
    /// </summary>
    public static class Geometry_Aerospike
    {
        /// <summary>
        /// Crea un aerospike completo con base toroidal.
        /// </summary>
        /// <param name="length">Longitud del spike [m]</param>
        /// <param name="baseRadius">Radio de la base toroidal [m]</param>
        /// <returns>Field3D con la geometria del aerospike</returns>
        public static Field3D Create(double length = 0.55, double baseRadius = 0.15)
        {
            // Spike conico truncado
            var spike = Field3D.Cone(0.02, 0.15, length)
                .Translate(0, 0, -length * 0.5);

            // Base toroidal (anillo de expansion)
            var baseTorus = Field3D.Torus(baseRadius, 0.03)
                .Translate(0, 0, length * 0.3);

            var aerospike = Field3D.Combine(spike, baseTorus);

            return aerospike;
        }

        /// <summary>
        /// Crea un aerospike con parametros de motor.
        /// </summary>
        public static Field3D Create(FFSC_PicoGK.Models.EngineParams p)
        {
            return Create(p.ChamberLength * 1.1, p.ExitRadius * 0.2);
        }
    }
}
