# FFSC Rocket Engine - Modular Pipeline Architecture

## Overview

This repository implements a full-flow staged combustion (FFSC) rocket engine design pipeline using a modular, graph-based execution architecture. Each computational component is implemented as an `ITask<TInput, TOutput>` node that can be composed into a declarative pipeline.

## Repository Structure

```
/workspaces/examples/
├── PipelineCore/                    # Core pipeline infrastructure
│   ├── ITask.cs                     # Generic task interface
│   ├── Node.cs                      # Graph node definition
│   ├── Graph.cs                     # Directed acyclic graph (DAG)
│   ├── Scheduler.cs                 # Topological execution engine
│   ├── TaskRegistry.cs              # Task registration and lookup
│   ├── PipelineLoader.cs            # JSON pipeline loader
│   ├── Exceptions.cs                # Custom exceptions
│   ├── TaskInputs.cs                # Composite input types
│   └── Interfaces/                  # Core contracts
│       ├── IEngineSpec.cs           # Engine specification
│       ├── IGeometry3D.cs           # 3D geometry generation
│       ├── ILattice3D.cs            # Lattice generation
│       ├── ICoolingField.cs         # Cooling analysis
│       ├── IField3D.cs              # 3D field analysis
│       └── IEngineFFSC.cs           # Engine assembly
│
├── FFSC-PicoGK/EngineFFSC/          # Primary FFSC implementation
│   ├── Models/                      # Data models
│   │   ├── EngineParams.cs          # Engine parameters (implements IEngineSpec)
│   │   ├── MaterialSpec.cs          # Material properties
│   │   ├── BartzCalculator.cs       # Heat transfer correlation
│   │   └── ThermoTables.cs          # Thermochemical tables
│   ├── Geometry/                    # Geometry generation
│   │   ├── Chamber/                 # Combustion chamber
│   │   │   ├── Geometry_Chamber.cs  # Chamber geometry (IGeometry3D)
│   │   │   ├── Lattice_DualLayer.cs # Dual-layer lattice (ILattice3D)
│   │   │   └── Lattice_Quasicrystal.cs # Quasicrystal lattice
│   │   ├── Nozzle/                  # Rao-optimized nozzle
│   │   │   └── Geometry_Nozzle.cs   # Nozzle geometry
│   │   ├── Aerospike/               # Linear aerospike
│   │   │   └── Geometry_Aerospike.cs
│   │   ├── Manifolds/               # Feed system manifolds
│   │   │   ├── Geometry_Manifold_FFSC.cs
│   │   │   ├── Geometry_Manifold_LOX.cs
│   │   │   └── Geometry_Manifold_CH4.cs
│   │   ├── Injectors/               # Coaxial injectors
│   │   │   └── Geometry_Injectors.cs
│   │   ├── Turbopump/               # Centrifugal turbopump
│   │   │   └── Geometry_Turbopump.cs
│   │   ├── Cooling/                 # Regenerative cooling
│   │   │   └── Geometry_Cooling.cs
│   │   ├── Pipes/                   # High-pressure feed pipes
│   │   │   └── Geometry_Pipes.cs
│   │   ├── Structural/              # Thrust frame
│   │   │   └── Geometry_Structural.cs
│   │   └── Supports/                # Engine mounts
│   │       └── Geometry_Supports.cs
│   ├── Physics/                     # Physics analysis
│   │   ├── Thermo/                  # Thermochemical analysis
│   │   │   └── ComputeThermoTask.cs # Thermodynamic state (IField3D)
│   │   ├── Structural/              # Structural analysis
│   │   │   └── ComputeThicknessTask.cs
│   │   ├── Stress/                  # Stress field
│   │   │   └── StressField.cs       # Volumetric stress (IField3D)
│   │   ├── CFD/                     # Simplified CFD
│   │   │   └── CFDTask.cs           # Thermal field (IField3D)
│   │   └── Cooling/                 # Cooling analysis
│   │       └── CoolingTask.cs       # Regenerative cooling (ICoolingField)
│   ├── Turbopump/                   # Turbopump design
│   │   ├── TurbopumpDesign.cs       # Parametric design
│   │   └── ShapeKernel_Turbopump.cs # Voxel-based turbopump
│   ├── Assembly/                    # Modular assembly
│   │   └── FFSC_Assembly_Modular.cs # Configurable assembly (IEngineFFSC)
│   ├── Pipeline/                    # Legacy pipeline
│   │   └── FFSC_Pipeline_Advanced.cs
│   ├── Versions/                    # Version wrappers
│   │   ├── FFSC_v03.cs
│   │   ├── FFSC_v04.cs
│   │   ├── FFSC_v05.cs
│   │   └── FFSC_v06.cs
│   ├── Utils/                       # Configuration loaders
│   │   ├── EngineParamsLoader.cs
│   │   ├── CoolingConfigLoader.cs
│   │   ├── GeometryConfigLoader.cs
│   │   ├── LatticeConfigLoader.cs
│   │   └── TurbopumpConfigLoader.cs
│   ├── Tasks/                       # PicoGK-compatible task wrappers
│   │   ├── Geometry/                # 10 geometry task files
│   │   ├── Physics/                 # 7 physics task files
│   │   └── Pipeline/
│   │       └── Task_Pipeline.cs
│   ├── Tests/
│   │   └── TestRunner.cs            # Custom test runner
│   └── FFSCShowcase_Advanced.cs     # Viewer entry point
│
├── EngineFFSC/                      # Legacy v06/v07 implementation
│   ├── Geometry/
│   ├── Physics/
│   ├── Pipeline/
│   ├── Tasks/
│   ├── Tests/
│   └── Versions/
│
├── ShapeKernel/                     # LEAP71 ShapeKernel (reference)
│   ├── BaseShapes/                  # Geometric primitives
│   ├── Frames/                      # Local coordinate frames
│   ├── Functions/                   # Boolean, offset, lattice ops
│   ├── Modulations/                 # Surface modulation
│   ├── Splines/                     # Spline operations
│   ├── Utilities/                   # Math utilities
│   └── Visualizations/              # Color maps, mesh painting
│
├── Pipeline/                        # Declarative pipeline definitions
│   └── pipeline.json                # FFSC advanced pipeline
│
├── Tests/vscode/                    # Unit tests
│   ├── Test_PipelineCore.cs         # Pipeline core tests
│   └── Test_AllNodes.cs             # Node integration tests
│
├── config/                          # JSON configuration files
│   ├── engine_params.json
│   ├── geometry_config.json
│   ├── cooling_config.json
│   ├── lattice_config.json
│   └── turbopump_config.json
│
├── docs/                            # Documentation
│   ├── ARCHITECTURE.md
│   ├── PIPELINE.md
│   ├── MODULES.md
│   └── GRAPH.md
│
├── Program.cs                       # Application entry point
├── PicoGKExamples.csproj            # Project file
└── PicoGK_Examples.sln              # Solution file
```

## Module Classification

### Nodes (ITask Implementations)
These are the executable units of the pipeline:

| Node ID | Name | Input | Output | Module |
|---------|------|-------|--------|--------|
| load_params | Load Engine Parameters | string (path) | EngineParams | Core |
| thermo | Thermodynamic Analysis | EngineParams | ThermoMap | Physics |
| thickness | Structural Thickness | EngineParams, ThermoMap | ThicknessMap | Physics |
| turbopump_design | Turbopump Design | EngineParams, double | TurbopumpDesign | Turbopump |
| geom_chamber | Chamber Geometry | EngineParams | Voxels | Geometry |
| geom_nozzle | Nozzle Geometry | EngineParams | Voxels | Geometry |
| geom_aerospike | Aerospike Geometry | Unit | Voxels | Geometry |
| geom_manifold_ffsc | FFSC Manifold | Unit | Voxels | Geometry |
| geom_manifold_lox | LOX Manifold | Unit | Voxels | Geometry |
| geom_manifold_ch4 | CH4 Manifold | Unit | Voxels | Geometry |
| geom_injectors | Injectors | EngineParams | Voxels | Geometry |
| geom_turbopump | Turbopump Geometry | EngineParams | Voxels | Geometry |
| geom_cooling | Cooling Channels | Voxels, Voxels | Voxels | Geometry |
| geom_pipes | Feed Pipes | Unit | Voxels | Geometry |
| geom_structural | Structural Frame | Unit | Voxels | Geometry |
| geom_supports | Engine Supports | Unit | Voxels | Geometry |
| physics_stress | Stress Field | Voxels, Voxels, Voxels | Voxels | Physics |
| physics_cfd | CFD Thermal | Voxels, Voxels, Voxels, Voxels | Voxels | Physics |
| cooling_analysis | Cooling Heat Transfer | EngineParams, ThermoMap | CoolingMap | Physics |
| lattice_dual | Dual-Layer Lattice | Voxels, double, double, double, double | Voxels | Geometry |
| lattice_quasi | Quasicrystal Lattice | Voxels, double, double | Voxels | Geometry |
| final_assembly | Final Engine Assembly | 16x Voxels | Voxels | Assembly |

### Modules (Folder Organization)

| Module | Folder | Responsibility | NuGet Ready |
|--------|--------|----------------|-------------|
| PipelineCore | PipelineCore/ | Core interfaces, graph, scheduler, registry | Yes |
| EngineSpec | FFSC-PicoGK/EngineFFSC/Models/ | Engine parameters, materials, calculators | Yes |
| Geometry | FFSC-PicoGK/EngineFFSC/Geometry/ | All geometry generation | Yes |
| Physics | FFSC-PicoGK/EngineFFSC/Physics/ | Thermodynamics, stress, CFD, cooling | Yes |
| Turbopump | FFSC-PicoGK/EngineFFSC/Turbopump/ | Turbopump design | Yes |
| Assembly | FFSC-PicoGK/EngineFFSC/Assembly/ | Modular engine assembly | Yes |
| Viewer | FFSC-PicoGK/EngineFFSC/ | PicoGK viewer integration | Yes |
| Utils | FFSC-PicoGK/EngineFFSC/Utils/ | Configuration loaders | Yes |
| ShapeKernel | ShapeKernel/ | LEAP71 geometry primitives (reference) | Yes |

### Resources (Data Files)

| Resource | Path | Purpose |
|----------|------|---------|
| Engine Parameters | config/engine_params.json | Master engine specification |
| Geometry Config | config/geometry_config.json | Per-component geometry params |
| Cooling Config | config/cooling_config.json | Cooling channel specifications |
| Lattice Config | config/lattice_config.json | Lattice generation params |
| Turbopump Config | config/turbopump_config.json | Turbopump design params |
| Pipeline Definition | Pipeline/pipeline.json | Declarative pipeline graph |
