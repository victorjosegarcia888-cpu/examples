// Task_Geometry_Chamber_v06.cs
//
// Task para generar geometria de camara de combustion v06.

using PicoGK;
using EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Chamber_v06
    {
        public static Voxels Task()
        {
            return Geometry_Chamber_v06.Create(new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                Lstar = 1.2,
                ContractionRatio = 6.0
            });
        }
    }
}
