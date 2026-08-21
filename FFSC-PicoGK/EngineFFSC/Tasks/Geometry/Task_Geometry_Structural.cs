// Task_Geometry_Structural.cs
//
// Task para generar geometria estructural.

using PicoGK;
using FFSC_PicoGK.Geometry.Structural;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de geometria estructural.
    /// </summary>
    public static class Task_Geometry_Structural
    {
        /// <summary>
        /// Genera la geometria estructural.
        /// </summary>
        public static Field3D Task()
        {
            return Geometry_Structural.Create(0.08, 0.10, 0.20, 0.40);
        }
    }
}
