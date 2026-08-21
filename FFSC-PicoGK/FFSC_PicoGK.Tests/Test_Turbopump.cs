// Test_Turbopump.cs
//
// Pruebas unitarias para turbobomba.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.EngineFFSC.Tasks.Physics;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Turbopump
    {
        [Fact]
        public void TurbopumpDesign_ReturnsValidGeometry()
        {
            EngineParams p = new EngineParams
            {
                MassFlowOxidizer = 320.0,
                MassFlowFuel = 89.0,
                ChamberPressure_bar = 350.0,
                TurbopumpRPM = 40000.0
            };

            var result = Task_Turbopump.Run(p);
            Assert.NotNull(result);
        }
    }
}
