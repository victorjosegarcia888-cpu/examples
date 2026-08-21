// Test_Cooling.cs
//
// Pruebas unitarias para cooling regenerativo.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.EngineFFSC.Tasks.Physics;
using PicoGK;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Cooling
    {
        [Fact]
        public void CoolingTask_ReturnsValidCooling()
        {
            Voxels chamber = new Voxels();
            Voxels spike = new Voxels();

            var result = Task_Cooling.Run(chamber, spike);
            Assert.NotNull(result);
        }
    }
}
