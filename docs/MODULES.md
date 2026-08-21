# Module Documentation

## PipelineCore

**Path**: `PipelineCore/`

**Purpose**: Core infrastructure for the modular pipeline execution framework.

**Components**:
- `ITask<TInput, TOutput>`: Generic task interface with strict typing
- `Node`: Graph node definition with metadata
- `Graph`: Directed acyclic graph with topological sort
- `Scheduler`: Execution engine that processes nodes in dependency order
- `TaskRegistry`: Central registry mapping IDs to task implementations
- `PipelineLoader`: JSON-based pipeline definition loader
- `TaskInputs.cs`: Composite input record types
- `Exceptions.cs`: Custom pipeline exceptions
- `Interfaces/`: Core contracts (IEngineSpec, IGeometry3D, ILattice3D, ICoolingField, IField3D, IEngineFFSC)

**Classification**: Module (foundational)

**Dependencies**: None

**NuGet Package**: `PipelineCore`

---

## NoyronAgents

**Path**: `FFSC-PicoGK/Agents/`

**Purpose**: Specialized internal agents for Noyron large nodes.

**Components**:
- `IAgent.cs`: Base agent interface
- `GeometryAgent.cs`: Geometry generation using existing geometry code
- `CoolingAgent.cs`: Cooling channel generation
- `LatticeAgent.cs`: Lattice structure generation (dual-layer, quasicrystal)
- `PhysicsAgent.cs`: Physics field computation (stress, thermal, CFD)
- `ValidationAgent.cs`: Geometry validation
- `FlowAgent.cs`: Flow field analysis
- `ImpellerAgent.cs`: Impeller design
- `ShaftAgent.cs`: Shaft design
- `CFDProxyAgent.cs`: CFD thermal analysis proxy
- `ThermalGradientAgent.cs`: Thermal gradient analysis
- `AssemblyAgent.cs`: Part assembly
- `InterfaceAgent.cs`: Interface handling
- `VisualizationAgent.cs`: PicoGK viewer conversion

**Classification**: Module (agent library)

**Dependencies**: PipelineCore, PicoGK, FFSC-PicoGK

**NuGet Package**: `FFSC.NoyronAgents`

---

## NoyronNodes

**Path**: `FFSC-PicoGK/Nodes/Noyron/`

**Purpose**: Large graph nodes representing complete engine subsystems with internal agents.

**Components**:
- `Nodo_CamaraCombustion.cs`: Combustion chamber (5 agents)
- `Nodo_PreBurner.cs`: Preburner (4 agents)
- `Nodo_ManifoldPrincipal.cs`: Main manifold (3 agents)
- `Nodo_Turbobomba.cs`: Turbopump (5 agents)
- `Nodo_CamposFisicos.cs`: Physics fields (2 agents)
- `Nodo_CoolingRegenerativo.cs`: Regenerative cooling (3 agents)
- `Nodo_LatticeAdaptativo.cs`: Adaptive lattice (2 agents)
- `Nodo_AssemblyFFSC.cs`: Engine assembly (3 agents)
- `Nodo_VisualizarMotor.cs`: Visualization (1 agent)

**Typed Inputs/Outputs**:

| Node | Input Type | Output Type |
|------|-----------|-------------|
| Nodo_CamaraCombustion | `Unit` | `Voxels` |
| Nodo_PreBurner | `Unit` | `Voxels` |
| Nodo_ManifoldPrincipal | `Unit` | `Voxels` |
| Nodo_Turbobomba | `Unit` | `Voxels` |
| Nodo_CamposFisicos | `CamposFisicosInput` | `Voxels` |
| Nodo_CoolingRegenerativo | `CoolingRegenerativoInput` | `Voxels` |
| Nodo_LatticeAdaptativo | `LatticeAdaptativoInput` | `Voxels` |
| Nodo_AssemblyFFSC | `AssemblyFFSCInput` | `Voxels` |
| Nodo_VisualizarMotor | `VisualizarMotorInput` | `Voxels` |

**Classification**: Module (node library)

**Dependencies**: PipelineCore, PicoGK, FFSC-PicoGK, NoyronAgents

**NuGet Package**: `FFSC.NoyronNodes`

---

## EngineSpec

**Path**: `FFSC-PicoGK/EngineFFSC/Models/`

**Purpose**: Engine specification, material properties, and engineering calculators.

**Components**:
- `EngineParams.cs`: Central parameter class (SI units)
- `MaterialSpec.cs`: Material properties (Inconel_718, etc.)
- `BartzCalculator.cs`: Bartz heat transfer correlation
- `ThermoTables.cs`: Thermochemical property tables

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK

**NuGet Package**: `FFSC.EngineSpec`

---

## Geometry

**Path**: `FFSC-PicoGK/EngineFFSC/Geometry/`

**Purpose**: 3D geometry generation for all engine subsystems.

**Components**:
- `Chamber/`: Combustion chamber geometry
- `Nozzle/`: Rao-optimized nozzle
- `Aerospike/`: Linear aerospike
- `Manifolds/`: LOX, CH4, FFSC manifolds
- `Injectors/`: Coaxial injectors
- `Turbopump/`: Centrifugal turbopump
- `Cooling/`: Regenerative cooling channels
- `Pipes/`: High-pressure feed pipes
- `Structural/`: Thrust frame
- `Supports/`: Engine mounts
- `Lattice_DualLayer.cs`: Dual-layer lattice
- `Lattice_Quasicrystal.cs`: Quasicrystal lattice

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK, EngineSpec

**NuGet Package**: `FFSC.Geometry`

---

## Physics

**Path**: `FFSC-PicoGK/EngineFFSC/Physics/`

**Purpose**: Physics and engineering analysis.

**Components**:
- `Thermo/ComputeThermoTask.cs`: Thermodynamic state
- `Structural/ComputeThicknessTask.cs`: Structural thickness
- `Stress/StressField.cs`: Volumetric stress field
- `CFD/CFDTask.cs`: Simplified CFD thermal
- `Cooling/CoolingTask.cs`: Regenerative cooling

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK, EngineSpec, Geometry

**NuGet Package**: `FFSC.Physics`

---

## Turbopump

**Path**: `FFSC-PicoGK/EngineFFSC/Turbopump/`

**Purpose**: Centrifugal turbopump parametric design.

**Components**:
- `TurbopumpDesign.cs`: Parametric design
- `ShapeKernel_Turbopump.cs`: Voxel-based turbopump

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK, EngineSpec, Geometry

**NuGet Package**: `FFSC.Turbopump`

---

## Assembly

**Path**: `FFSC-PicoGK/EngineFFSC/Assembly/`

**Purpose**: Modular engine assembly with configurable subsystems.

**Components**:
- `FFSC_Assembly_Modular.cs`: Configurable assembly
- `FFSC_Assembly_Config`: Boolean flags for subsystems

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK, Geometry, Physics, Turbopump

**NuGet Package**: `FFSC.Assembly`

---

## Viewer

**Path**: `FFSC-PicoGK/EngineFFSC/`

**Purpose**: PicoGK runtime viewer integration.

**Components**:
- `FFSCShowcase_Advanced.cs`: Entry points for viewer

**Classification**: Module

**Dependencies**: PipelineCore, PicoGK, Assembly

**NuGet Package**: `FFSC.Viewer`

---

## Utils

**Path**: `FFSC-PicoGK/EngineFFSC/Utils/`

**Purpose**: Configuration file loaders.

**Components**:
- `EngineParamsLoader.cs`: JSON loader for engine parameters
- `CoolingConfigLoader.cs`: JSON loader for cooling configuration
- `GeometryConfigLoader.cs`: JSON loader for geometry configuration
- `LatticeConfigLoader.cs`: JSON loader for lattice configuration
- `TurbopumpConfigLoader.cs`: JSON loader for turbopump configuration

**Classification**: Module

**Dependencies**: EngineSpec

**NuGet Package**: `FFSC.Utils`

---

## ShapeKernel

**Path**: `ShapeKernel/`

**Purpose**: LEAP71 ShapeKernel geometry primitives (reference implementation).

**Components**:
- `BaseShapes/`: Sphere, cylinder, box, cone, pipe, ring, lattice manifold, lattice pipe
- `Frames/`: Local coordinate frames
- `Functions/`: Boolean operations, offset, lattice functions
- `Modulations/`: Surface and line modulation
- `Splines/`: Control point splines and surfaces
- `Utilities/`: Vector operations, bisection, mesh utilities
- `Visualizations/`: Color palettes, mesh painting

**Classification**: Module (reference)

**Dependencies**: PicoGK

**NuGet Package**: `Leap71.ShapeKernel`

---

## LegacyCode

**Path**: `EngineFFSC/`

**Purpose**: Legacy v06/v07 implementation for reference.

**Classification**: Resource (legacy reference)

**Dependencies**: Varies

---

## Tests

**Path**: `Tests/vscode/`

**Purpose**: Unit and integration tests.

**Components**:
- `Test_PipelineCore.cs`: Pipeline core tests
- `Test_AllNodes.cs`: Node integration tests

**Classification**: Resource (tests)

**Dependencies**: PipelineCore, FFSC-PicoGK

---

## Config

**Path**: `config/`

**Purpose**: JSON configuration files.

**Files**:
- `engine_params.json`: Master engine specification
- `geometry_config.json`: Per-component geometry params
- `cooling_config.json`: Cooling channel specifications
- `lattice_config.json`: Lattice generation params
- `turbopump_config.json`: Turbopump design params

**Classification**: Resource (configuration)

---

## Pipeline

**Path**: `Pipeline/`

**Purpose**: Declarative pipeline definitions.

**Files**:
- `pipeline.json`: Canonical Noyron pipeline graph

**Classification**: Resource (pipeline definition)

---

## Docs

**Path**: `docs/`

**Purpose**: Documentation.

**Files**:
- `ARCHITECTURE.md`: System architecture
- `PIPELINE.md`: Pipeline execution details
- `MODULES.md`: Module descriptions
- `GRAPH.md`: Graph dependencies
- `PERFORMANCE.md`: Optimization guide
- `IMPLEMENTATION.md`: Implementation summary
- `README.md`: Quick start
- `SUMMARY.md`: Implementation complete summary

**Classification**: Resource (documentation)
