// Task_Geometry_Nozzle.cs
//
// Task para generar geometria de tobera Rao-optimizada.

using PicoGK;
using FFSC_PicoGK.Geometry.Nozzle;

namespace FFSC_PicoGK.Tasks.Geometry
{
    public static class Task_Geometry_Nozzle
    {
        public static Voxels Task()
        {
            return Geometry_Nozzle.Create(
                new FFSC_PicoGK.Models.EngineParams
                {
                    ThroatRadius = 0.12,
                    ExitRadius = 0.80,
                    ExpansionRatio = 45.0,
                    Lstar = 1.2
                });
        }
    }
}
