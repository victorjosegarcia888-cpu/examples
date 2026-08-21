// Task_Cooling.cs
//
// Task para calculo de refrigeracion regenerativa.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Cooling;

namespace FFSC_PicoGK.Tasks.Physics
{
    public static class Task_Cooling
    {
        public static CoolingMap Task()
        {
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

            var thermo = ComputeThermoTask.Run(p);
            return CoolingTask.Run(p, thermo);
        }
    }
}
