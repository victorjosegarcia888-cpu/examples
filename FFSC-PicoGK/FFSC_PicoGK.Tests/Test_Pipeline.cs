// Test_Pipeline.cs
//
// Pruebas unitarias para pipeline completo.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Pipeline;
using FFSC_PicoGK.EngineFFSC.Assembly;
using PicoGK;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Pipeline
    {
        [Fact]
        public void Pipeline_Advanced_Executes()
        {
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

            Voxels pipelineResult = FFSC_Pipeline_Advanced.Execute(p);
            Assert.NotNull(pipelineResult);
        }

        [Fact]
        public void Assembly_Versions_AllBuild()
        {
            Voxels v03 = FFSC_Assembly_Modular.V03();
            Assert.NotNull(v03);

            Voxels v04 = FFSC_Assembly_Modular.V04();
            Assert.NotNull(v04);

            Voxels v05 = FFSC_Assembly_Modular.V05();
            Assert.NotNull(v05);

            Voxels v06 = FFSC_Assembly_Modular.V06();
            Assert.NotNull(v06);
        }
    }
}
