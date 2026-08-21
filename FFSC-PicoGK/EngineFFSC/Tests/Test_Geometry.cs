// Test_Geometry.cs
//
// Pruebas unitarias para geometria FFSC.

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
    public static class Test_Geometry
    {
        public static bool Run()
        {
            bool passed = true;
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
            passed &= chamber != null;

            Voxels nozzle = Geometry_Nozzle.Create(p);
            passed &= nozzle != null;

            Voxels spike = Geometry_Aerospike.Create(p);
            passed &= spike != null;

            Voxels lox = Geometry_Manifold_LOX.Create();
            Voxels ch4 = Geometry_Manifold_CH4.Create();
            Voxels ffsc = Geometry_Manifold_FFSC.Create();
            passed &= lox != null && ch4 != null && ffsc != null;

            Voxels injectors = Geometry_Injectors.Create();
            passed &= injectors != null;

            Voxels turbopump = Geometry_Turbopump.Create();
            passed &= turbopump != null;

            Voxels pipes = Geometry_Pipes.Create();
            passed &= pipes != null;

            Voxels structural = Geometry_Structural.Create();
            passed &= structural != null;

            Voxels supports = Geometry_Supports.Create();
            passed &= supports != null;

            Voxels cooling = Geometry_Cooling.Primary(chamber, spike);
            passed &= cooling != null;

            return passed;
        }
    }
}
