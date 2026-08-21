// Task_Geometry_Injectors_v07.cs
//
// Task para generar geometria de inyectores v07.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Injectors_v07
    {
        public static Voxels Task()
        {
            return Geometry_Injectors_v07.Build(new EngineParams
            {
                MixtureRatio = 3.6,
                ChamberRadius = 0.35
            });
        }
    }
}
