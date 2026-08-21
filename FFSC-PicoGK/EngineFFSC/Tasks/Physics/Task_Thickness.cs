// Task_Thickness.cs
//
// Task para calculo de espesor estructural.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;

namespace FFSC_PicoGK.Tasks.Physics
{
    public static class Task_Thickness
    {
        public static ThicknessMap Task()
        {
            EngineParams p = new EngineParams
            {
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberPressure_bar = 350.0,
                Lstar = 1.2,
                ExpansionRatio = 45.0,
                Nz = 300,
                SafetyFactor = 1.5
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            var thermo = ComputeThermoTask.Run(p);
            return ComputeThicknessTask.Run(p, thermo);
        }
    }
}
