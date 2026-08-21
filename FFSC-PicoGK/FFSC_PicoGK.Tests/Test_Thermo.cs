// Test_Thermo.cs
//
// Pruebas unitarias para termoquimica.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Thermo
    {
        [Fact]
        public void ComputeThermo_ReturnsValidResults()
        {
            EngineParams p = new EngineParams
            {
                Thrust = 2_500_000.0,
                ChamberPressure_bar = 350.0,
                ExpansionRatio = 45.0,
                MixtureRatio = 3.6,
                MassFlowOxidizer = 320.0,
                MassFlowFuel = 89.0
            };

            var result = ComputeThermoTask.Run(p);
            Assert.NotNull(result);
            Assert.True(result.ChamberTemperature_K > 3000);
            Assert.True(result.Cstar_m_s > 2000);
        }
    }
}
