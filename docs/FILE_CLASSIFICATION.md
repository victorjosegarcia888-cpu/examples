# File Classification

## Classification Legend
- **NODE**: Implements `ITask<TInput, TOutput>` - executable pipeline unit
- **MODULE**: Folder/grouping of related functionality
- **RESOURCE**: Data file, configuration, or non-executable asset
- **AGENT**: Internal agent for Noyron large nodes (implements `IAgent`)

---

## PipelineCore (Module)

| File | Classification | Purpose |
|------|---------------|---------|
| `ITask.cs` | MODULE | Core task interface definition |
| `Node.cs` | MODULE | Graph node definition |
| `Graph.cs` | MODULE | DAG with topological sort |
| `Scheduler.cs` | MODULE | Deterministic execution engine |
| `TaskRegistry.cs` | MODULE | Task registration and lookup |
| `PipelineLoader.cs` | MODULE | JSON pipeline loader |
| `TaskInputs.cs` | MODULE | Composite input record types |
| `Exceptions.cs` | MODULE | Custom pipeline exceptions |
| `Interfaces/IGeometry3D.cs` | MODULE | Geometry generation contract |
| `Interfaces/ILattice3D.cs` | MODULE | Lattice generation contract |
| `Interfaces/ICoolingField.cs` | MODULE | Cooling analysis contract |
| `Interfaces/IField3D.cs` | MODULE | 3D field analysis contract |
| `Interfaces/IEngineFFSC.cs` | MODULE | Engine assembly contract |
| `Interfaces/IEngineSpec.cs` | MODULE | Engine specification contract |

---

## FFSC-PicoGK/Agents (Module - NoyronAgents)

| File | Classification | Purpose |
|------|---------------|---------|
| `IAgent.cs` | AGENT | Base agent interface |
| `GeometryAgent.cs` | AGENT | Geometry generation using existing code |
| `CoolingAgent.cs` | AGENT | Cooling channel generation |
| `LatticeAgent.cs` | AGENT | Lattice structure generation |
| `PhysicsAgent.cs` | AGENT | Physics field computation |
| `ValidationAgent.cs` | AGENT | Geometry validation |
| `FlowAgent.cs` | AGENT | Flow field analysis |
| `ImpellerAgent.cs` | AGENT | Impeller design |
| `ShaftAgent.cs` | AGENT | Shaft design |
| `CFDProxyAgent.cs` | AGENT | CFD thermal analysis proxy |
| `ThermalGradientAgent.cs` | AGENT | Thermal gradient analysis |
| `AssemblyAgent.cs` | AGENT | Part assembly |
| `InterfaceAgent.cs` | AGENT | Interface handling |
| `VisualizationAgent.cs` | AGENT | PicoGK viewer conversion |

---

## FFSC-PicoGK/Nodes/Noyron (Module - NoyronNodes)

| File | Classification | Input | Output | Internal Agents |
|------|---------------|-------|--------|-----------------|
| `Nodo_CamaraCombustion.cs` | NODE | `Unit` | `Voxels` | Geometry, Cooling, Lattice, Physics, Validation |
| `Nodo_PreBurner.cs` | NODE | `Unit` | `Voxels` | Geometry, Cooling, Lattice, Physics |
| `Nodo_ManifoldPrincipal.cs` | NODE | `Unit` | `Voxels` | Geometry, Flow, Validation |
| `Nodo_Turbobomba.cs` | NODE | `Unit` | `Voxels` | Geometry, Impeller, Shaft, Cooling, Physics |
| `Nodo_CamposFisicos.cs` | NODE | `CamposFisicosInput` | `Voxels` | Physics, CFDProxy |
| `Nodo_CoolingRegenerativo.cs` | NODE | `CoolingRegenerativoInput` | `Voxels` | Cooling, Physics, Flow |
| `Nodo_LatticeAdaptativo.cs` | NODE | `LatticeAdaptativoInput` | `Voxels` | Lattice, ThermalGradient |
| `Nodo_AssemblyFFSC.cs` | NODE | `AssemblyFFSCInput` | `Voxels` | Assembly, Interface, Validation |
| `Nodo_VisualizarMotor.cs` | NODE | `VisualizarMotorInput` | `Voxels` | Visualization |

---

## FFSC-PicoGK/Pipeline (Module)

| File | Classification | Purpose |
|------|---------------|---------|
| `PipelineBuilder.cs` | MODULE | Graph builder, InputFactory, task registration |
| `FFSC_Pipeline_Advanced.cs` | NODE | Legacy advanced pipeline executor |
| `Nodes/LoadParamsNode.cs` | NODE | Load engine parameters from JSON |
| `Nodes/ThermoNode.cs` | NODE | Thermodynamic analysis |
| `Nodes/ThicknessNode.cs` | NODE | Structural thickness calculation |
| `Nodes/CoolingAnalysisNode.cs` | NODE | Cooling heat transfer analysis |
| `Nodes/TurbopumpDesignNode.cs` | NODE | Turbopump parametric design |
| `Nodes/GeometryChamberNode.cs` | NODE | Chamber geometry wrapper |
| `Nodes/GeometryNozzleNode.cs` | NODE | Nozzle geometry wrapper |
| `Nodes/GeometryAerospikeNode.cs` | NODE | Aerospike geometry wrapper |
| `Nodes/GeometryManifoldFFSCNode.cs` | NODE | FFSC manifold wrapper |
| `Nodes/GeometryManifoldLOXNode.cs` | NODE | LOX manifold wrapper |
| `Nodes/GeometryManifoldCH4Node.cs` | NODE | CH4 manifold wrapper |
| `Nodes/GeometryInjectorsNode.cs` | NODE | Injectors wrapper |
| `Nodes/GeometryTurbopumpNode.cs` | NODE | Turbopump geometry wrapper |
| `Nodes/GeometryCoolingNode.cs` | NODE | Cooling channels wrapper |
| `Nodes/GeometryPipesNode.cs` | NODE | Feed pipes wrapper |
| `Nodes/GeometryStructuralNode.cs` | NODE | Structural frame wrapper |
| `Nodes/GeometrySupportsNode.cs` | NODE | Supports wrapper |
| `Nodes/StressFieldNode.cs` | NODE | Stress field wrapper |
| `Nodes/CFDNode.cs` | NODE | CFD thermal wrapper |
| `Nodes/LatticeDualNode.cs` | NODE | Dual-layer lattice wrapper |
| `Nodes/LatticeQuasiNode.cs` | NODE | Quasicrystal lattice wrapper |
| `Nodes/AssemblyNode.cs` | NODE | Final assembly wrapper |
| `Nodes/GeometryStructuralNode.cs` | NODE | Structural geometry wrapper |

---

## FFSC-PicoGK/EngineFFSC/Models (Module - EngineSpec)

| File | Classification | Purpose |
|------|---------------|---------|
| `EngineParams.cs` | MODULE | Central engine parameters (SI units) |
| `MaterialSpec.cs` | MODULE | Material properties |
| `BartzCalculator.cs` | MODULE | Bartz heat transfer correlation |
| `ThermoTables.cs` | MODULE | Thermochemical property tables |
| `VdbExporter.cs` | MODULE | VDB export utility |

---

## FFSC-PicoGK/EngineFFSC/Geometry (Module - Geometry)

| File | Classification | Input | Output |
|------|---------------|-------|--------|
| `Chamber/Geometry_Chamber.cs` | NODE | `EngineParams` | `Voxels` |
| `Chamber/Lattice_DualLayer.cs` | NODE | `Voxels, double, double, double, double` | `Voxels` |
| `Chamber/Lattice_Quasicrystal.cs` | NODE | `Voxels, double, double` | `Voxels` |
| `Nozzle/Geometry_Nozzle.cs` | NODE | `EngineParams` | `Voxels` |
| `Aerospike/Geometry_Aerospike.cs` | NODE | `EngineParams` | `Voxels` |
| `Manifolds/Geometry_Manifold_FFSC.cs` | NODE | `Unit` | `Voxels` |
| `Manifolds/Geometry_Manifold_LOX.cs` | NODE | `Unit` | `Voxels` |
| `Manifolds/Geometry_Manifold_CH4.cs` | NODE | `Unit` | `Voxels` |
| `Injectors/Geometry_Injectors.cs` | NODE | `Unit` | `Voxels` |
| `Turbopump/Geometry_Turbopump.cs` | NODE | `Unit` | `Voxels` |
| `Cooling/Geometry_Cooling.cs` | NODE | `Voxels, Voxels` | `Voxels` |
| `Pipes/Geometry_Pipes.cs` | NODE | `Unit` | `Voxels` |
| `Structural/Geometry_Structural.cs` | NODE | `Unit` | `Voxels` |
| `Supports/Geometry_Supports.cs` | NODE | `Unit` | `Voxels` |

---

## FFSC-PicoGK/EngineFFSC/Physics (Module - Physics)

| File | Classification | Input | Output |
|------|---------------|-------|--------|
| `Thermo/ComputeThermoTask.cs` | NODE | `EngineParams` | `ThermoMap` |
| `Structural/ComputeThicknessTask.cs` | NODE | `EngineParams, ThermoMap` | `ThicknessMap` |
| `Stress/StressField.cs` | NODE | `Voxels, Voxels, Voxels` | `Voxels` |
| `CFD/CFDTask.cs` | NODE | `Voxels` | `Voxels` |
| `Cooling/CoolingTask.cs` | NODE | `EngineParams, ThermoMap` | `CoolingMap` |

---

## FFSC-PicoGK/EngineFFSC/Turbopump (Module - Turbopump)

| File | Classification | Input | Output |
|------|---------------|-------|--------|
| `TurbopumpDesign.cs` | NODE | `EngineParams, double` | `TurbopumpDesign` |
| `ShapeKernel_Turbopump.cs` | NODE | `EngineParams` | `Voxels` |

---

## FFSC-PicoGK/EngineFFSC/Assembly (Module - Assembly)

| File | Classification | Input | Output |
|------|---------------|-------|--------|
| `FFSC_Assembly_Modular.cs` | NODE | `EngineParams, AssemblyConfig` | `Voxels` |

---

## FFSC-PicoGK/EngineFFSC/Tasks (Module - Task Wrappers)

| File | Classification | Input | Output |
|------|---------------|-------|--------|
| `Geometry/Task_Geometry_Chamber.cs` | NODE | `EngineParams` | `Voxels` |
| `Geometry/Task_Geometry_Nozzle.cs` | NODE | `EngineParams` | `Voxels` |
| `Geometry/Task_Geometry_Aerospike.cs` | NODE | `EngineParams` | `Voxels` |
| `Geometry/Task_Geometry_Manifolds.cs` | NODE | `Unit` | `Voxels` |
| `Geometry/Task_Geometry_Injectors.cs` | NODE | `Unit` | `Voxels` |
| `Geometry/Task_Geometry_Turbopump.cs` | NODE | `EngineParams` | `Voxels` |
| `Geometry/Task_Geometry_Cooling.cs` | NODE | `Voxels, Voxels` | `Voxels` |
| `Geometry/Task_Geometry_Pipes.cs` | NODE | `Unit` | `Voxels` |
| `Geometry/Task_Geometry_Structural.cs` | NODE | `Unit` | `Voxels` |
| `Geometry/Task_Geometry_Supports.cs` | NODE | `Unit` | `Voxels` |
| `Physics/Task_Thermo.cs` | NODE | `EngineParams` | `ThermoMap` |
| `Physics/Task_Stress.cs` | NODE | `Voxels, Voxels, Voxels` | `Voxels` |
| `Physics/Task_CFD.cs` | NODE | `Voxels` | `Voxels` |
| `Physics/Task_Cooling.cs` | NODE | `EngineParams, ThermoMap` | `CoolingMap` |
| `Physics/Task_Lattice.cs` | NODE | `Voxels` | `Voxels` |
| `Physics/Task_Thickness.cs` | NODE | `EngineParams, ThermoMap` | `ThicknessMap` |
| `Physics/Task_Turbopump.cs` | NODE | `EngineParams, double` | `TurbopumpDesign` |
| `Pipeline/Task_Pipeline.cs` | NODE | `Unit` | `Voxels` |

---

## FFSC-PicoGK/EngineFFSC/Versions (Module - Version Wrappers)

| File | Classification | Purpose |
|------|---------------|---------|
| `FFSC_v03.cs` | NODE | Version v03 preset |
| `FFSC_v04.cs` | NODE | Version v04 preset |
| `FFSC_v05.cs` | NODE | Version v05 preset |
| `FFSC_v06.cs` | NODE | Version v06 preset |

---

## FFSC-PicoGK/EngineFFSC/Utils (Module - Utils)

| File | Classification | Purpose |
|------|---------------|---------|
| `EngineParamsLoader.cs` | MODULE | JSON loader for engine parameters |
| `CoolingConfigLoader.cs` | MODULE | JSON loader for cooling config |
| `GeometryConfigLoader.cs` | MODULE | JSON loader for geometry config |
| `LatticeConfigLoader.cs` | MODULE | JSON loader for lattice config |
| `TurbopumpConfigLoader.cs` | MODULE | JSON loader for turbopump config |

---

## FFSC-PicoGK/EngineFFSC/Tests (Resource - Tests)

| File | Classification | Purpose |
|------|---------------|---------|
| `TestRunner.cs` | RESOURCE | Custom test runner |

---

## EngineFFSC (Module - Legacy)

| File | Classification | Purpose |
|------|---------------|---------|
| `Geometry/**/*.cs` | NODE | Legacy geometry implementations |
| `Physics/**/*.cs` | NODE | Legacy physics implementations |
| `Tasks/**/*.cs` | NODE | Legacy task wrappers |
| `Pipeline/**/*.cs` | MODULE | Legacy pipeline definitions |
| `Versions/**/*.cs` | NODE | Legacy version wrappers |

---

## ShapeKernel (Module - Reference)

| File | Classification | Purpose |
|------|---------------|---------|
| `BaseShapes/*.cs` | MODULE | Geometric primitives |
| `Frames/*.cs` | MODULE | Local coordinate frames |
| `Functions/*.cs` | MODULE | Boolean, offset, lattice ops |
| `Modulations/*.cs` | MODULE | Surface modulation |
| `Splines/*.cs` | MODULE | Spline operations |
| `Utilities/*.cs` | MODULE | Math utilities |
| `Visualizations/*.cs` | MODULE | Color maps, mesh painting |

---

## GeometryII (Module)

| File | Classification | Purpose |
|------|---------------|---------|
| `Tasks/GeometryII_ShapeGenerator.cs` | NODE | Shape kernel integration |

---

## Top-Level Files

| File | Classification | Purpose |
|------|---------------|---------|
| `Program.cs` | MODULE | Application entry point |
| `PicoGKExamples.csproj` | RESOURCE | Project file |
| `PicoGK_Examples.sln` | RESOURCE | Solution file |

---

## Configuration Files (Resources)

| File | Purpose |
|------|---------|
| `config/engine_params.json` | Master engine specification |
| `config/geometry_config.json` | Per-component geometry params |
| `config/cooling_config.json` | Cooling channel specifications |
| `config/lattice_config.json` | Lattice generation params |
| `config/turbopump_config.json` | Turbopump design params |
| `FFSC-PicoGK/config/*.json` | Additional configuration files |

---

## Pipeline Definitions (Resources)

| File | Purpose |
|------|---------|
| `Pipeline/pipeline.json` | Canonical Noyron pipeline graph |

---

## Test Files (Resources)

| File | Purpose |
|------|---------|
| `Tests/vscode/Test_PipelineCore.cs` | Pipeline core unit tests |
| `Tests/vscode/Test_AllNodes.cs` | Node integration tests |

---

## Documentation (Resources)

| File | Purpose |
|------|---------|
| `docs/ARCHITECTURE.md` | System architecture |
| `docs/PIPELINE.md` | Pipeline execution details |
| `docs/MODULES.md` | Module descriptions |
| `docs/GRAPH.md` | Graph dependencies |
| `docs/PERFORMANCE.md` | Optimization guide |
| `docs/IMPLEMENTATION.md` | Implementation summary |
| `docs/README.md` | Quick start |
| `docs/SUMMARY.md` | Implementation complete summary |
| `README.md` | Project README |
| `LICENSE.md` | License file |

---

## Summary Counts

| Classification | Count |
|---------------|-------|
| NODE | ~50+ |
| MODULE | ~15 |
| RESOURCE | ~30+ |
| AGENT | 14 |
