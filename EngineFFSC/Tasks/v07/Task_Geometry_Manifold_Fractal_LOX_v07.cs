// Task_Geometry_Manifold_Fractal_LOX_v07.cs
//
// Task para generar manifold fractal LOX v07.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Manifold_Fractal_LOX_v07
    {
        public static Voxels Task()
        {
            return Geometry_Manifold_Fractal_LOX_v07.Build(
                trunkRadius: 0.18,
                length: 0.32,
                depth: 3,
                bifurcationAngle: 0.785);
        }
    }
}
