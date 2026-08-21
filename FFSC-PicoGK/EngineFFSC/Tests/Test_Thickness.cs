// Test_Thickness.cs
//
// Pruebas unitarias para espesor estructural.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Thickness
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
                SafetyFactor = 1.5
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            ThermoMap thermo = ComputeThermoTask.Run(p);
            var thickness = ComputeThicknessTask.Run(p, thermo);

            passed &= thickness != null;
            passed &= thickness.Points != null;
            passed &= thickness.Points.Length == p.Nz;

            // Verificar que espesor > 0
            foreach (var pt in thickness.Points)
            {
                passed &= pt.Thickness > 0.0;
                passed &= pt.Radius > 0.0;
            }

            return passed;
        }
    }
}
