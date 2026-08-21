// Task_Geometry_Manifold_LOX_v06.cs
//
// Task para generar manifold LOX v06.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Manifold_LOX_v06
    {
        public static Voxels Task()
        {
            return Geometry_Manifold_LOX_v06.Create(
                radius: 0.2,
                length: 0.4,
                branches: 6);
        }
    }
}
