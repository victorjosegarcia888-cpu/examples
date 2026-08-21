// Test_Lattice.cs
//
// Pruebas unitarias para lattice adaptativo.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.EngineFFSC.Tasks.Physics;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Lattice
    {
        [Fact]
        public void GenerateLattice_ReturnsValidLattice()
        {
            var lattice = Task_Lattice.Run();
            Assert.NotNull(lattice);
        }
    }
}
