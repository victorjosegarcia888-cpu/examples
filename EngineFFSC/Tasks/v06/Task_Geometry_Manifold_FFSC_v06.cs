// Task_Geometry_Manifold_FFSC_v06.cs
//
// Task para generar manifold FFSC v06.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Manifold_FFSC_v06
    {
        public static Voxels Task()
        {
            return Geometry_Manifold_FFSC_v06.Create(
                innerRadius: 0.25,
                outerRadius: 0.35,
                length: 0.6);
        }
    }
}
