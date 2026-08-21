// Task_Geometry_Pipes.cs
//
// Task para generar tuberias de alta presion.

using PicoGK;
using FFSC_PicoGK.Geometry.Pipes;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de tuberias.
    /// </summary>
    public static class Task_Geometry_Pipes
    {
        /// <summary>
        /// Genera la red de tuberias.
        /// </summary>
        public static Field3D Task()
        {
            return Geometry_Pipes.Create(0.03, 0.03, 0.80, 0.60);
        }
    }
}
