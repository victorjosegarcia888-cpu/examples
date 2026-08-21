// Task_Geometry_Manifolds.cs
//
// Task para generar todas las geometrias de manifold.

using PicoGK;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Tasks.Geometry
{
    /// <summary>
    /// Task de generacion de manifolds.
    /// </summary>
    public static class Task_Geometry_Manifolds
    {
        /// <summary>
        /// Genera todos los manifolds.
        /// </summary>
        public static Field3D Task()
        {
            var lox = Geometry_Manifold_LOX.Create();
            var ch4 = Geometry_Manifold_CH4.Create();
            var ffsc = Geometry_Manifold_FFSC.Create();

            return Field3D.Combine(lox, ch4, ffsc);
        }
    }
}
