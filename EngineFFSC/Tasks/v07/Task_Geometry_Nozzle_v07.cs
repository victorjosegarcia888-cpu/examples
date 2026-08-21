// Task_Geometry_Nozzle_v07.cs
//
// Task para generar geometria de tobera v07.

using PicoGK;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Nozzle_v07
    {
        public static Voxels Task()
        {
            return Geometry_Nozzle.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                Lstar = 1.2
            });
        }
    }
}
