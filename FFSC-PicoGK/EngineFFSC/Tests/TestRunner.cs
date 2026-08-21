// TestRunner.cs
//
// Simple test runner for FFSC_PicoGK without external test framework dependencies.
// Run with: dotnet run --property:RunTests=true

using PicoGK;
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
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;
using FFSC_PicoGK.Tasks.Physics;
using FFSC_PicoGK.Pipeline;
using FFSC_PicoGK.EngineFFSC.Assembly;

namespace FFSC_PicoGK.EngineFFSC.Tests
{
    public static class TestRunner
    {
        public static int RunAll()
        {
            int passed = 0;
            int failed = 0;

            void AssertTrue(string name, bool condition)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] {name}");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] {name}");
                    failed++;
                }
            }

            // Geometry tests
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
                AssertTrue("Chamber.Create", chamber != null);

                Voxels nozzle = Geometry_Nozzle.Create(p);
                AssertTrue("Nozzle.Create", nozzle != null);

                Voxels spike = Geometry_Aerospike.Create(p);
                AssertTrue("Aerospike.Create", spike != null);

                Voxels lox = Geometry_Manifold_LOX.Create();
                AssertTrue("Manifold_LOX.Create", lox != null);

                Voxels ch4 = Geometry_Manifold_CH4.Create();
                AssertTrue("Manifold_CH4.Create", ch4 != null);

                Voxels ffsc = Geometry_Manifold_FFSC.Create();
                AssertTrue("Manifold_FFSC.Create", ffsc != null);

                Voxels injectors = Geometry_Injectors.Create();
                AssertTrue("Injectors.Create", injectors != null);

                Voxels turbopump = Geometry_Turbopump.Create();
                AssertTrue("Turbopump.Create", turbopump != null);

                Voxels pipes = Geometry_Pipes.Create();
                AssertTrue("Pipes.Create", pipes != null);

                Voxels structural = Geometry_Structural.Create();
                AssertTrue("Structural.Create", structural != null);

                Voxels supports = Geometry_Supports.Create();
                AssertTrue("Supports.Create", supports != null);

                Voxels cooling = Geometry_Cooling.Primary(null, null);
                AssertTrue("Cooling.Primary", cooling != null);
            }

            // Thermo test
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

                var thermo = ComputeThermoTask.Run(p);
                AssertTrue("ComputeThermoTask.Run", thermo != null);
                if (thermo != null && thermo.Points != null)
                {
                    AssertTrue("Thermo.ChamberTemp > 3000K", thermo.Points[0].Tg > 3000);
                }
            }

            // Thickness test
            {
                EngineParams p = new EngineParams
                {
                    ChamberPressure_bar = 350.0,
                    ThroatRadius = 0.12,
                    ExitRadius = 0.80,
                    ChamberRadius = 0.35
                };

                var thermo = ComputeThermoTask.Run(p);
                var thicknessMap = ComputeThicknessTask.Run(p, thermo);
                AssertTrue("ComputeThicknessTask.Run != null", thicknessMap != null);
            }

            // Pipeline test
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

                Voxels pipeline = FFSC_Pipeline_Advanced.Execute(p);
                AssertTrue("FFSC_Pipeline_Advanced.Execute", pipeline != null);
            }

            // Assembly versions test
            {
                Voxels v03 = FFSC_Assembly_Modular.V03();
                AssertTrue("Assembly.V03", v03 != null);

                Voxels v04 = FFSC_Assembly_Modular.V04();
                AssertTrue("Assembly.V04", v04 != null);

                Voxels v05 = FFSC_Assembly_Modular.V05();
                AssertTrue("Assembly.V05", v05 != null);

                Voxels v06 = FFSC_Assembly_Modular.V06();
                AssertTrue("Assembly.V06", v06 != null);
            }

            Console.WriteLine($"\n=== Results: {passed} passed, {failed} failed ===");
            return failed == 0 ? 0 : 1;
        }
    }
}
