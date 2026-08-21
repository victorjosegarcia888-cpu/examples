// Test_Geometry.cs
//
// Pruebas unitarias para geometria FFSC.

using Xunit;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Geometry.Chamber;
using FFSC_PicoGK.Geometry.Nozzle;
using FFSC_PicoGK.Geometry.Aerospike;
using FFSC_PicoGK.Geometry.Manifolds;
using FFSC_PicoGK.Geometry.Injectors;
using FFSC_PicoGK.Geometry.Turbopump;
using FFSC_PicoGK.Geometry.Cooling;
using FFSC_PicoGK.Geometry.Pipes;
using FFSC_PicoGK.Geometry.Structural;
using FFSC_PicoGK.Geometry.Supports;
using PicoGK;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public class Test_Geometry
    {
        [Fact]
        public void Chamber_Nozzle_Aerospike_Created()
        {
            EngineParams p = new EngineParams
            {
                ChamberRadius = 0.35,
                ChamberLength = 0.50,
                ThroatRadius = 0.12,
                ExitRadius = 0.80,
                Lstar = 1.2,
                ExpansionRatio = 45.0,
                ContractionRatio = 6.0
            };

            Voxels chamber = Geometry_Chamber.Create(p);
            Assert.NotNull(chamber);

            Voxels nozzle = Geometry_Nozzle.Create(p);
            Assert.NotNull(nozzle);

            Voxels spike = Geometry_Aerospike.Create(p);
            Assert.NotNull(spike);
        }

        [Fact]
        public void Manifolds_Created()
        {
            Voxels lox = Geometry_Manifold_LOX.Create();
            Assert.NotNull(lox);

            Voxels ch4 = Geometry_Manifold_CH4.Create();
            Assert.NotNull(ch4);

            Voxels ffsc = Geometry_Manifold_FFSC.Create();
            Assert.NotNull(ffsc);
        }

        [Fact]
        public void Injectors_Turbopump_Pipes_Created()
        {
            Voxels injectors = Geometry_Injectors.Create();
            Assert.NotNull(injectors);

            Voxels turbopump = Geometry_Turbopump.Create();
            Assert.NotNull(turbopump);

            Voxels pipes = Geometry_Pipes.Create();
            Assert.NotNull(pipes);
        }

        [Fact]
        public void Structural_Supports_Cooling_Created()
        {
            Voxels structural = Geometry_Structural.Create();
            Assert.NotNull(structural);

            Voxels supports = Geometry_Supports.Create();
            Assert.NotNull(supports);

            EngineParams p = new EngineParams();
            Voxels cooling = Geometry_Cooling.Primary(null, null);
            Assert.NotNull(cooling);
        }
    }
}
