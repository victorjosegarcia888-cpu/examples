// Task_Geometry_Injectors_v06.cs
//
// Task para generar geometria de inyectores v06.

using PicoGK;
using EngineFFSC.Geometry;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Injectors_v06
    {
        public static Voxels Task()
        {
            return Geometry_Injectors_v06.Create(
                count: 60,
                pitch: 0.006);
        }
    }
}
