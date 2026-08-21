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
- `Interfaces/`: Core contracts (IEngineSpec, IGeometry3D, ILattice3D, etc.)

**Dependencies**: None (foundational module)

**NuGet Package**: `PipelineCore`

## EngineSpec

**Path**: `FFSC-PicoGK/EngineFFSC/Models/`

**Purpose**: Engine specification, material properties, and engineering calculators.

**Components**:
- `EngineParams`: Central parameter class (SI units)
- `MaterialSpec`: Material properties (Inconel_718, etc.)
- `BartzCalculator`: Bartz heat transfer correlation
- `ThermoTables`: Thermochemical property tables (Cp mixture)

**Inputs**: JSON configuration files
**Outputs**: EngineParams, MaterialSpec, calculation results

**Dependencies**: PipelineCore, PicoGK

**NuGet Package**: `FFSC.EngineSpec`

## Geometry

**Path**: `FFSC-PicoGK/EngineFFSC/Geometry/`

**Purpose**: 3D geometry generation for all engine subsystems.

**Components**:
- `Chamber/`: Spherical-cylindrical combustion chamber
- `Nozzle/`: Rao-optimized parabolic nozzle
- `Aerospike/`: Linear aerospike with toroidal base
- `Manifolds/`: LOX, CH4, and FFSC preburner manifolds
- `Injectors/`: Coaxial swirl injectors
- `Turbopump/`: Centrifugal pump rotor and housing
- `Cooling/`: Regenerative helical cooling channels
- `Pipes/`: High-pressure feed system pipes
- `Structural/`: Thrust frame and gimbal mounts
- `Supports/`: Engine support struts
- `Lattice_DualLayer.cs`: Stress-adaptive dual-layer lattice
- `Lattice_Quasicrystal.cs`: Golden-ratio quasicrystal lattice

**Inputs**: EngineParams, dependency voxels
**Outputs**: Voxels (voxel-based geometry)

**Dependencies**: PipelineCore, PicoGK, EngineSpec

**NuGet Package**: `FFSC.Geometry`

## Physics

**Path**: `FFSC-PicoGK/EngineFFSC/Physics/`

**Purpose**: Physics and engineering analysis.

**Components**:
- `Thermo/ComputeThermoTask.cs`: Thermodynamic state (TAD, Tg(z), Bartz hg(z), Qnorm(z))
- `Structural/ComputeThicknessTask.cs`: Barlow's formula + thermal margin
- `Stress/StressField.cs`: Volumetric stress field (static/dynamic)
- `CFD/CFDTask.cs`: Simplified CFD (static/dynamic thermal fields)
- `Cooling/CoolingTask.cs`: Regenerative cooling heat transfer

**Inputs**: EngineParams, ThermoMap, Voxels
**Outputs**: ThermoMap, ThicknessMap, CoolingMap, Voxels

**Dependencies**: PipelineCore, PicoGK, EngineSpec, Geometry

**NuGet Package**: `FFSC.Physics`

## Turbopump

**Path**: `FFSC-PicoGK/EngineFFSC/Turbopump/`

**Purpose**: Centrifugal turbopump parametric design.

**Components**:
- `TurbopumpDesign.cs`: Euler equation, velocity triangles, specific speed
- `ShapeKernel_Turbopump.cs`: Voxel-based turbopump shape generation

**Inputs**: EngineParams, mass flow rate
**Outputs**: TurbopumpDesign, Voxels

**Dependencies**: PipelineCore, PicoGK, EngineSpec, Geometry

**NuGet Package**: `FFSC.Turbopump`

## Assembly

**Path**: `FFSC-PicoGK/EngineFFSC/Assembly/`

**Purpose**: Modular engine assembly with configurable subsystems.

**Components**:
- `FFSC_Assembly_Modular.cs`: Configurable assembly with version presets (V03-V06)
- `FFSC_Assembly_Config`: Boolean flags for each subsystem

**Inputs**: EngineParams, AssemblyConfig
**Outputs**: Voxels (complete engine)

**Dependencies**: PipelineCore, PicoGK, Geometry, Physics, Turbopump

**NuGet Package**: `FFSC.Assembly`

## Viewer

**Path**: `FFSC-PicoGK/EngineFFSC/`

**Purpose**: PicoGK runtime viewer integration.

**Components**:
- `FFSCShowcase_Advanced.cs`: Entry points for viewer
- `Task()`: Default v06 build
- `Task(string version)`: Version selector
- `Task_Subsystem(string subsystem)`: Individual subsystem viewer
- `Task_Pipeline()`: Full pipeline execution

**Inputs**: None (uses default or config)
**Outputs**: void (adds to viewer)

**Dependencies**: PipelineCore, PicoGK, Assembly

**NuGet Package**: `FFSC.Viewer`

## Utils

**Path**: `FFSC-PicoGK/EngineFFSC/Utils/`

**Purpose**: Configuration file loaders.

**Components**:
- `EngineParamsLoader.cs`: JSON loader for engine parameters
- `CoolingConfigLoader.cs`: JSON loader for cooling configuration
- `GeometryConfigLoader.cs`: JSON loader for geometry configuration
- `LatticeConfigLoader.cs`: JSON loader for lattice configuration
- `TurbopumpConfigLoader.cs`: JSON loader for turbopump configuration

**Inputs**: File paths
**Outputs**: Configuration objects

**Dependencies**: EngineSpec

**NuGet Package**: `FFSC.Utils`

## ShapeKernel

**Path**: `ShapeKernel/`

**Purpose**: LEAP71 ShapeKernel geometry primitives (reference implementation).

**Components**:
- `BaseShapes/`: Sphere, cylinder, box, cone, pipe, ring, lattice manifold, lattice pipe
- `Frames/`: Local coordinate frames
- `Functions/`: Boolean operations, offset, lattice functions, voxel functions
- `Modulations/`: Surface and line modulation
- `Splines/`: Control point splines and surfaces
- `Utilities/`: Vector operations, bisection, CSV writing, mesh utilities
- `Visualizations/`: Color palettes, mesh painting, rotation animation

**Note**: Requires PicoGK version with `PicoGK.Numerics` namespace. Current PicoGK 1.7.7.4 is compatible with core BaseShape functionality.

**Dependencies**: PicoGK

**NuGet Package**: `Leap71.ShapeKernel`
