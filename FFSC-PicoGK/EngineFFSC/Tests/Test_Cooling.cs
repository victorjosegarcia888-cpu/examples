// Test_Cooling.cs
//
// Pruebas unitarias para refrigeracion.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Cooling;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Cooling
    {
        public static bool Run()
        {
            bool passed = true;

            EngineParams p = new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberPressure_bar = 350.0,
                Lstar = 1.2,
                ExpansionRatio = 45.0,
                Nz = 300,
                CoolantMassFlow = 50.0,
                CoolantInletTemp_C = 20.0,
                CoolantOutletTemp_C = 750.0
            };

            ThermoMap thermo = ComputeThermoTask.Run(p);
            CoolingMap coolingMap = CoolingTask.Run(p, thermo);

            passed &= coolingMap != null;
            passed &= coolingMap.Points != null;
            passed &= coolingMap.Points.Length == p.Nz;

            foreach (var pt in coolingMap.Points)
            {
                passed &= pt.Tw > 0.0;
                passed &= pt.CoolantTemp > 0.0;
            }

            return passed;
        }
    }
}
