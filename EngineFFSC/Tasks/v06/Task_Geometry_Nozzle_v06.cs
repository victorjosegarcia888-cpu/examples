// Task_Geometry_Nozzle_v06.cs
//
// Task para generar geometria de tobera v06.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Nozzle_v06
    {
        public static Voxels Task()
        {
            return Geometry_Nozzle_v06.Create(new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                Lstar = 1.2,
                ChamberLength = 0.50
            });
        }
    }
}
