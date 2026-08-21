// Test_Thickness.cs
//
// Pruebas unitarias para espesor estructural.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Structural;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Thickness
    {
        [Fact]
        public void ComputeThickness_ReturnsValidThickness()
        {
            EngineParams p = new EngineParams
            {
                ChamberPressure_bar = 350.0,
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                ChamberRadius = 0.35
            };

            double thickness = ComputeThicknessTask.Run(p);
            Assert.True(thickness > 0);
            Assert.True(thickness < 0.1);
        }
    }
}
