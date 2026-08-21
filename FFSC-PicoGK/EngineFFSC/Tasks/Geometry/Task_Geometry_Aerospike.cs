// Task_Geometry_Aerospike.cs
//
// Task para generar geometria de aerospike lineal.

using PicoGK;
using FFSC_PicoGK.Geometry.Aerospike;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de geometria de aerospike.
    /// </summary>
    public static class Task_Geometry_Aerospike
    {
        /// <summary>
        /// Genera el aerospike.
        /// </summary>
        public static Field3D Task()
        {
            return Geometry_Aerospike.Create(0.55, 0.15);
        }
    }
}
