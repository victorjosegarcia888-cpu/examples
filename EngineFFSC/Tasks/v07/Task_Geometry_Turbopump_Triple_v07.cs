// Task_Geometry_Turbopump_Triple_v07.cs
//
// Task para generar geometria de turbobomba triple v07.

using PicoGK;
using FFSC_PicoGK.EngineFFSC.Geometry;
using FFSC_PicoGK.Models;

namespace EngineFFSC.Tasks
{
    public static class Task_Geometry_Turbopump_Triple_v07
    {
        public static Voxels Task()
        {
            EngineParams p = new EngineParams
            {
                TurbopumpRPM = 40000.0
            };

            return Geometry_Turbopump_Triple_v07.Build(p);
        }
    }
}

}
