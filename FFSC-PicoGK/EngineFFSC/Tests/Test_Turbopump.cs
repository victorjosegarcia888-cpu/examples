// Test_Turbopump.cs
//
// Pruebas unitarias para diseno de turbobomba.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.EngineFFSC.Turbopump;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Turbopump
    {
        public static bool Run()
        {
            bool passed = true;

            EngineParams p = new EngineParams
            {
                MassFlowOxidizer = 320.0,
                TurbopumpRPM = 40000.0
            };

            var design = TurbopumpDesigner.Run(p, p.MassFlowOxidizer);

            passed &= design != null;
            passed &= design.MassFlow > 0.0;
            passed &= design.Head > 0.0;
            passed &= design.Omega > 0.0;
            passed &= design.R2 > design.R1;
            passed &= design.BladeCount > 0;

            return passed;
        }
    }
}
