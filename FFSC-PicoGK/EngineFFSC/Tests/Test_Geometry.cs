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

            // Test camara
            Field3D chamber = Geometry_Chamber.Create(p);
            passed &= chamber != null;

            // Test tobera
            Field3D nozzle = Geometry_Nozzle.Create(p);
            passed &= nozzle != null;

            // Test aerospike
            Field3D spike = Geometry_Aerospike.Create(p);
            passed &= spike != null;

            // Test manifolds
            Field3D lox = Geometry_Manifold_LOX.Create();
            Field3D ch4 = Geometry_Manifold_CH4.Create();
            Field3D ffsc = Geometry_Manifold_FFSC.Create();
            passed &= lox != null && ch4 != null && ffsc != null;

            // Test inyectores
            Field3D injectors = Geometry_Injectors.Create();
            passed &= injectors != null;

            // Test turbobomba
            Field3D turbopump = Geometry_Turbopump.Create();
            passed &= turbopump != null;

            // Test tuberias
            Field3D pipes = Geometry_Pipes.Create();
            passed &= pipes != null;

            // Test estructural
            Field3D structural = Geometry_Structural.Create();
            passed &= structural != null;

            // Test soportes
            Field3D supports = Geometry_Supports.Create();
            passed &= supports != null;

            // Test refrigeracion
            Field3D cooling = Geometry_Cooling.Primary(chamber, spike);
            passed &= cooling != null;

            return passed;
        }
    }
}
