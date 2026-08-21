// Task_Geometry_Injectors.cs
//
// Task para generar geometria de inyectores.

using PicoGK;
using FFSC_PicoGK.Geometry.Injectors;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de geometria de inyectores.
    /// </summary>
    public static class Task_Geometry_Injectors
    {
        /// <summary>
        /// Genera la placa de inyectores.
        /// </summary>
        public static Field3D Task()
        {
            return Geometry_Injectors.Create(32, 0.24, 0.008, 0.004, 0.06);
        }
    }
}
