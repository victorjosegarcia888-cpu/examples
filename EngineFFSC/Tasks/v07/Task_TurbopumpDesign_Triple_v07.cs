// Task_TurbopumpDesign_Triple_v07.cs
//
// Task para diseno de turbobomba triple v07.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_TurbopumpDesign_Triple_v07
    {
        public static Voxels Task()
        {
            EngineParams p = new EngineParams
            {
                MassFlowOxidizer = 320.0,
                MassFlowFuel = 89.0,
                TurbopumpRPM = 40000.0
            };

            Physics_Turbopump_Triple_v07.Run(p);
            return new Voxels();
        }
    }
}
