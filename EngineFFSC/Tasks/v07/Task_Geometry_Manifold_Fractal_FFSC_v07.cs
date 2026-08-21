// Task_Geometry_Manifold_Fractal_FFSC_v07.cs
//
// Task para generar manifold fractal FFSC completo v07.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Manifold_Fractal_FFSC_v07
    {
        public static Voxels Task()
        {
            return Geometry_Manifold_Fractal_FFSC_v07.Build(
                radius: 0.20,
                length: 0.40,
                branchCount: 6,
                preburnerLineRadius: 0.05,
                returnLineRadius: 0.04);
        }
    }
}
