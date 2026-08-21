// Test_AllNodes.cs
//
// Integration tests for all node wrappers implementing ITask<TInput, TOutput>.
// Each test validates the node's Id, Name, and Execute method.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.Physics.Structural;
using FFSC_PicoGK.EngineFFSC.Turbopump;
using FFSC_PicoGK.Pipeline.Nodes;

namespace Tests.vscode;

public static class Test_AllNodes
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

        void AssertEqual<T>(string name, T expected, T actual)
        {
            bool equal = EqualityComparer<T>.Default.Equals(expected, actual);
            if (equal)
            {
                Console.WriteLine($"[PASS] {name}");
            }
            else
            {
                Console.WriteLine($"[FAIL] {name} (expected: {expected}, actual: {actual})");
                failed++;
            }
        }

        void AssertNotNull<T>(string name, T? value) where T : class
        {
            AssertTrue(name, value != null);
        }

        EngineParams CreateEngineParams()
        {
            return new EngineParams
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
                MixtureRatio = 3.6,
                Cstar = 2400.0,
                Isp = 380.0,
                MassFlowOxidizer = 320.0,
                MassFlowFuel = 89.0,
                SafetyFactor = 1.5,
                Nz = 300
            };
        }

        // LoadParamsNode
        {
            var node = new LoadParamsNode();
            AssertEqual("LoadParamsNode.Id", "load_params", node.Id);
            AssertEqual("LoadParamsNode.Name", "Load Engine Parameters", node.Name);

            EngineParams? result = node.Execute("config/engine_params.json");
            AssertNotNull("LoadParamsNode.Execute", result);
            if (result != null)
            {
                AssertTrue("LoadParamsNode.ChamberPressure > 0", result.ChamberPressure_bar > 0);
            }
        }

        // ThermoNode
        {
            var node = new ThermoNode();
            AssertEqual("ThermoNode.Id", "thermo", node.Id);
            AssertEqual("ThermoNode.Name", "Thermodynamic Analysis", node.Name);

            var input = new ThermoTaskInput(CreateEngineParams());
            ThermoMap? result = node.Execute(input);
            AssertNotNull("ThermoNode.Execute", result);
            if (result != null && result.Points != null && result.Points.Length > 0)
            {
                AssertTrue("ThermoNode.Tg > 3000K", result.Points[0].Tg > 3000);
            }
        }

        // ThicknessNode
        {
            var node = new ThicknessNode();
            AssertEqual("ThicknessNode.Id", "thickness", node.Id);
            AssertEqual("ThicknessNode.Name", "Structural Thickness Calculation", node.Name);

            var thermoNode = new ThermoNode();
            var thermoInput = new ThermoTaskInput(CreateEngineParams());
            ThermoMap? thermo = thermoNode.Execute(thermoInput);

            var thicknessInput = new ThicknessTaskInput(CreateEngineParams(), thermo!);
            ThicknessMap? result = node.Execute(thicknessInput);
            AssertNotNull("ThicknessNode.Execute", result);
        }

        // TurbopumpDesignNode
        {
            var node = new TurbopumpDesignNode();
            AssertEqual("TurbopumpDesignNode.Id", "turbopump_design", node.Id);
            AssertEqual("TurbopumpDesignNode.Name", "Turbopump Design", node.Name);

            var input = new TurbopumpTaskInput(CreateEngineParams(), 320.0);
            TurbopumpDesign? result = node.Execute(input);
            AssertNotNull("TurbopumpDesignNode.Execute", result);
        }

        // GeometryChamberNode
        {
            var node = new GeometryChamberNode();
            AssertEqual("GeometryChamberNode.Id", "geom_chamber", node.Id);

            var input = new ThermoTaskInput(CreateEngineParams());
            Voxels? result = node.Execute(input);
            AssertNotNull("GeometryChamberNode.Execute", result);
        }

        // GeometryNozzleNode
        {
            var node = new GeometryNozzleNode();
            AssertEqual("GeometryNozzleNode.Id", "geom_nozzle", node.Id);

            var input = new ThermoTaskInput(CreateEngineParams());
            Voxels? result = node.Execute(input);
            AssertNotNull("GeometryNozzleNode.Execute", result);
        }

        // GeometryAerospikeNode
        {
            var node = new GeometryAerospikeNode();
            AssertEqual("GeometryAerospikeNode.Id", "geom_aerospike", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryAerospikeNode.Execute", result);
        }

        // GeometryManifoldFFSCNode
        {
            var node = new GeometryManifoldFFSCNode();
            AssertEqual("GeometryManifoldFFSCNode.Id", "geom_manifold_ffsc", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryManifoldFFSCNode.Execute", result);
        }

        // GeometryManifoldLOXNode
        {
            var node = new GeometryManifoldLOXNode();
            AssertEqual("GeometryManifoldLOXNode.Id", "geom_manifold_lox", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryManifoldLOXNode.Execute", result);
        }

        // GeometryManifoldCH4Node
        {
            var node = new GeometryManifoldCH4Node();
            AssertEqual("GeometryManifoldCH4Node.Id", "geom_manifold_ch4", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryManifoldCH4Node.Execute", result);
        }

        // GeometryInjectorsNode
        {
            var node = new GeometryInjectorsNode();
            AssertEqual("GeometryInjectorsNode.Id", "geom_injectors", node.Id);

            var input = new ThermoTaskInput(CreateEngineParams());
            Voxels? result = node.Execute(input);
            AssertNotNull("GeometryInjectorsNode.Execute", result);
        }

        // GeometryTurbopumpNode
        {
            var node = new GeometryTurbopumpNode();
            AssertEqual("GeometryTurbopumpNode.Id", "geom_turbopump", node.Id);

            var input = new ThermoTaskInput(CreateEngineParams());
            Voxels? result = node.Execute(input);
            AssertNotNull("GeometryTurbopumpNode.Execute", result);
        }

        // GeometryCoolingNode
        {
            var node = new GeometryCoolingNode();
            AssertEqual("GeometryCoolingNode.Id", "geom_cooling", node.Id);

            var chamberNode = new GeometryChamberNode();
            var spikeNode = new GeometryAerospikeNode();
            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);

            var input = new CoolingGeometryInput(chamber!, spike!);
            Voxels? result = node.Execute(input);
            AssertNotNull("GeometryCoolingNode.Execute", result);
        }

        // GeometryPipesNode
        {
            var node = new GeometryPipesNode();
            AssertEqual("GeometryPipesNode.Id", "geom_pipes", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryPipesNode.Execute", result);
        }

        // GeometryStructuralNode
        {
            var node = new GeometryStructuralNode();
            AssertEqual("GeometryStructuralNode.Id", "geom_structural", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometryStructuralNode.Execute", result);
        }

        // GeometrySupportsNode
        {
            var node = new GeometrySupportsNode();
            AssertEqual("GeometrySupportsNode.Id", "geom_supports", node.Id);

            Voxels? result = node.Execute(Unit.Value);
            AssertNotNull("GeometrySupportsNode.Execute", result);
        }

        // StressFieldNode
        {
            var node = new StressFieldNode();
            AssertEqual("StressFieldNode.Id", "physics_stress", node.Id);

            var chamberNode = new GeometryChamberNode();
            var spikeNode = new GeometryAerospikeNode();
            var manifoldNode = new GeometryManifoldFFSCNode();

            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);
            Voxels? manifold = manifoldNode.Execute(Unit.Value);

            var input = new StressFieldInput(chamber!, spike!, manifold!);
            Voxels? result = node.Execute(input);
            AssertNotNull("StressFieldNode.Execute", result);
        }

        // CFDNode
        {
            var node = new CFDNode();
            AssertEqual("CFDNode.Id", "physics_cfd", node.Id);

            var chamberNode = new GeometryChamberNode();
            var nozzleNode = new GeometryNozzleNode();
            var spikeNode = new GeometryAerospikeNode();
            var manifoldNode = new GeometryManifoldFFSCNode();

            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? nozzle = nozzleNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);
            Voxels? manifold = manifoldNode.Execute(Unit.Value);

            var input = new CFDInput(chamber!, nozzle!, spike!, manifold!);
            Voxels? result = node.Execute(input);
            AssertNotNull("CFDNode.Execute", result);
        }

        // LatticeDualNode
        {
            var node = new LatticeDualNode();
            AssertEqual("LatticeDualNode.Id", "lattice_dual", node.Id);

            var stressNode = new StressFieldNode();
            var chamberNode = new GeometryChamberNode();
            var spikeNode = new GeometryAerospikeNode();
            var manifoldNode = new GeometryManifoldFFSCNode();

            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);
            Voxels? manifold = manifoldNode.Execute(Unit.Value);

            Voxels? stress = stressNode.Execute(new StressFieldInput(chamber!, spike!, manifold!));

            var input = new LatticeDualInput(stress!, 0.6, 0.3, 0.015, 0.008);
            Voxels? result = node.Execute(input);
            AssertNotNull("LatticeDualNode.Execute", result);
        }

        // LatticeQuasiNode
        {
            var node = new LatticeQuasiNode();
            AssertEqual("LatticeQuasiNode.Id", "lattice_quasi", node.Id);

            var stressNode = new StressFieldNode();
            var chamberNode = new GeometryChamberNode();
            var spikeNode = new GeometryAerospikeNode();
            var manifoldNode = new GeometryManifoldFFSCNode();

            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);
            Voxels? manifold = manifoldNode.Execute(Unit.Value);

            Voxels? stress = stressNode.Execute(new StressFieldInput(chamber!, spike!, manifold!));

            var input = new LatticeQuasiInput(stress!, 0.3, 0.5);
            Voxels? result = node.Execute(input);
            AssertNotNull("LatticeQuasiNode.Execute", result);
        }

        // AssemblyNode
        {
            var node = new AssemblyNode();
            AssertEqual("AssemblyNode.Id", "final_assembly", node.Name);

            var chamberNode = new GeometryChamberNode();
            var nozzleNode = new GeometryNozzleNode();
            var spikeNode = new GeometryAerospikeNode();
            var manifoldFFSCNode = new GeometryManifoldFFSCNode();
            var manifoldLOXNode = new GeometryManifoldLOXNode();
            var manifoldCH4Node = new GeometryManifoldCH4Node();
            var injectorsNode = new GeometryInjectorsNode();
            var turbopumpNode = new GeometryTurbopumpNode();
            var coolingNode = new GeometryCoolingNode();
            var pipesNode = new GeometryPipesNode();
            var structuralNode = new GeometryStructuralNode();
            var supportsNode = new GeometrySupportsNode();
            var stressNode = new StressFieldNode();
            var cfdNode = new CFDNode();
            var latticeDualNode = new LatticeDualNode();
            var latticeQuasiNode = new LatticeQuasiNode();

            Voxels? chamber = chamberNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? nozzle = nozzleNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? spike = spikeNode.Execute(Unit.Value);
            Voxels? manifoldFFSC = manifoldFFSCNode.Execute(Unit.Value);
            Voxels? manifoldLOX = manifoldLOXNode.Execute(Unit.Value);
            Voxels? manifoldCH4 = manifoldCH4Node.Execute(Unit.Value);
            Voxels? injectors = injectorsNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? turbopumpGeom = turbopumpNode.Execute(new ThermoTaskInput(CreateEngineParams()));
            Voxels? cooling = coolingNode.Execute(new CoolingGeometryInput(chamber!, spike!));
            Voxels? pipes = pipesNode.Execute(Unit.Value);
            Voxels? structural = structuralNode.Execute(Unit.Value);
            Voxels? supports = supportsNode.Execute(Unit.Value);
            Voxels? stress = stressNode.Execute(new StressFieldInput(chamber!, spike!, manifoldFFSC!));
            Voxels? cfd = cfdNode.Execute(new CFDInput(chamber!, nozzle!, spike!, manifoldFFSC!));
            Voxels? latticeDual = latticeDualNode.Execute(new LatticeDualInput(stress!, 0.6, 0.3, 0.015, 0.008));
            Voxels? latticeQuasi = latticeQuasiNode.Execute(new LatticeQuasiInput(stress!, 0.3, 0.5));

            var assemblyInput = new AssemblyInput(
                chamber!, nozzle!, spike!, manifoldFFSC!, manifoldLOX!, manifoldCH4!,
                injectors!, turbopumpGeom!, cooling!, pipes!, structural!, supports!,
                stress!, cfd!, latticeDual!, latticeQuasi!);

            Voxels? result = node.Execute(assemblyInput);
            AssertNotNull("AssemblyNode.Execute", result);
        }

        Console.WriteLine($"\n=== All Nodes Tests: {passed} passed, {failed} failed ===");
        return failed == 0 ? 0 : 1;
    }
}
