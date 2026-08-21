// Task_Geometry_Supports.cs
//
// Task para generar soportes del motor.

using PicoGK;
using FFSC_PicoGK.Geometry.Supports;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de soportes.
    /// </summary>
    public static class Task_Geometry_Supports
    {
        /// <summary>
        /// Genera la estructura de soportes.
        /// </summary>
        public static Field3D Task()
        {
            return Geometry_Supports.Create(4, 0.03, 0.40, 0.50);
        }
    }
}
