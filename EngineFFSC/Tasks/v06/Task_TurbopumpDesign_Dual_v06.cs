// Task_TurbopumpDesign_Dual_v06.cs
//
// Task para diseno de turbobomba dual v06.

using PicoGK;
using FFSC_PicoGK.Models;
using EngineFFSC.Physics;

namespace EngineFFSC.Tasks
{
    public static class Task_TurbopumpDesign_Dual_v06
    {
        public static Voxels Task()
        {
            EngineParams p = new EngineParams
            {
                MassFlowOxidizer = 320.0,
                MassFlowFuel = 89.0,
                TurbopumpRPM = 40000.0
            };

            Physics_Turbopump_Dual_v06.AnalyzeDual(p.MassFlowOxidizer, p.MassFlowFuel, 30e6, 20e6, p.TurbopumpRPM);
            return new Voxels();
        }
    }
}
