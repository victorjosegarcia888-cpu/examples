// Task_Turbopump.cs
//
// Task para diseno de turbobomba.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.EngineFFSC.Turbopump;

namespace FFSC_PicoGK.Tasks.Physics
{
    public static class Task_Turbopump
    {
        public static TurbopumpDesign Task()
        {
            EngineParams p = new EngineParams
            {
                MassFlowOxidizer = 320.0,
                TurbopumpRPM = 40000.0
            };

            return TurbopumpDesigner.Run(p, p.MassFlowOxidizer);
        }
    }
}
