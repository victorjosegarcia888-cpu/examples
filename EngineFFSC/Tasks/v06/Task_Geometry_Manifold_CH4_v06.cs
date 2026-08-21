// Task_Geometry_Manifold_CH4_v06.cs
//
// Task para generar manifold CH4 v06.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Manifold_CH4_v06
    {
        public static Voxels Task()
        {
            return Geometry_Manifold_CH4_v06.Create(
                radius: 0.18,
                length: 0.35,
                branches: 8);
        }
    }
}
