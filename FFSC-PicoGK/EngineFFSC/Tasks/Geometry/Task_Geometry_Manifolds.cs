// Task_Geometry_Manifolds.cs
//
// Task para generar todas las geometrias de manifold.

using PicoGK;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Manifolds
    {
        public static Voxels Task()
        {
            var lox = Geometry_Manifold_LOX.Create();
            var ch4 = Geometry_Manifold_CH4.Create();
            var ffsc = Geometry_Manifold_FFSC.Create();
            return lox + ch4 + ffsc;
        }
    }
}
