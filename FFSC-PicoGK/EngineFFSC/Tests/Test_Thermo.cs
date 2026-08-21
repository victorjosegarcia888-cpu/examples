// Test_Thermo.cs
//
// Pruebas unitarias para termoquimica FFSC.

using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Thermo
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
                Nz = 300
            };

            ThermoMap map = ComputeThermoTask.Run(p);

            passed &= map != null;
            passed &= map.Points != null;
            passed &= map.Points.Length == p.Nz;

            // Verificar que Tad esta en rango razonable
            double Tad = map.Points[0].Tg;
            passed &= Tad > 2000.0 && Tad < 5000.0;

            // Verificar que hg > 0
            double hg = map.Points[0].Hg;
            passed &= hg > 0.0;

            // Verificar Qnorm en [0,1]
            double qnorm = map.Points[0].Qnorm;
            passed &= qnorm >= 0.0 && qnorm <= 1.0;

            return passed;
        }
    }
}
