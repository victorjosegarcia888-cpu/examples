// Test_Pipeline.cs
//
// Pruebas unitarias para pipeline completo.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Pipeline;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class Test_Pipeline
    {
        public static bool Run()
        {
            bool passed = true;

            EngineParams p = new EngineParams
            {
                Thrust = 2_500_000.0,
                ChamberPressure_bar = 350.0,
                ExpansionRatio = 45.0,
                Lstar = 1.2,
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ContractionRatio = 6.0,
                MassFlowOxidizer = 320.0,
                TurbopumpRPM = 40000.0
            };

            p.Material = new MaterialSpec
            {
                Name = "Inconel_718",
                YieldStrengthPa = 1.03e9
            };

            // Test pipeline completo
            Field3D pipelineResult = FFSC_Pipeline_Advanced.Execute(p);
            passed &= pipelineResult != null;

            // Test ensamblado v03
            Field3D v03 = FFSC_Assembly_Modular.V03();
            passed &= v03 != null;

            // Test ensamblado v04
            Field3D v04 = FFSC_Assembly_Modular.V04();
            passed &= v04 != null;

            // Test ensamblado v05
            Field3D v05 = FFSC_Assembly_Modular.V05();
            passed &= v05 != null;

            // Test ensamblado v06
            Field3D v06 = FFSC_Assembly_Modular.V06();
            passed &= v06 != null;

            return passed;
        }
    }
}
