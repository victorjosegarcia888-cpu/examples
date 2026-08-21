// Task_Geometry_Chamber_v07.cs
//
// Task para generar geometria de camara de combustion v07.

using PicoGK;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Chamber_v07
    {
        public static Voxels Task()
        {
            return Geometry_Chamber.Create(new EngineParams
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
