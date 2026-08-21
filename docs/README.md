# Modular FFSC Rocket Engine Design System

## System Overview

This repository implements a full-flow staged combustion (FFSC) rocket engine design system using a modular, graph-based execution pipeline. The architecture is inspired by Noyron/Powder's node-graph approach, adapted for C# and PicoGK.

## Core Concepts

### 1. ITask<TInput, TOutput>
Every computation is a pure function with strictly typed inputs and outputs:
```csharp
public interface ITask<in TInput, out TOutput>
{
    string Id { get; }
    string Name { get; }
    TOutput Execute(TInput input);
}
```

### 2. Node
A node wraps an ITask with metadata for graph construction:
```csharp
public class Node
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? TaskType { get; set; }
    public List<string> DependsOn { get; set; }
    public Dictionary<string, object?> Input { get; set; }
    public string? OutputKey { get; set; }
}
```

### 3. Graph
A directed acyclic graph (DAG) with topological sort and cycle detection.

### 4. Scheduler
Executes nodes in topological order, resolving dependencies and passing typed outputs.

### 5. TaskRegistry
Central registry mapping unique IDs to task implementations.

### 6. PipelineLoader
Loads pipeline definitions from JSON files.

## Repository Structure

```
/workspaces/examples/
├── PipelineCore/                    # Core pipeline infrastructure
│   ├── ITask.cs                     # Generic task interface
│   ├── Node.cs                      # Graph node definition
│   ├── Graph.cs                     # DAG with topological sort
│   ├── Scheduler.cs                 # Deterministic execution engine
│   ├── TaskRegistry.cs              # Task registration
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
│   ├── Geometry/                    # Geometry generation
│   ├── Physics/                     # Physics analysis
│   ├── Turbopump/                   # Turbopump design
│   ├── Assembly/                    # Modular assembly
│   ├── Pipeline/                    # Node wrappers + builder
│   ├── Utils/                       # Configuration loaders
│   ├── Tests/                       # Test runner
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
├── GeometryII/                      # GeometryII module
│   └── Tasks/
│       └── GeometryII_ShapeGenerator.cs
│
├── ShapeKernel/                     # LEAP71 ShapeKernel (reference)
│   ├── BaseShapes/
│   ├── Frames/
│   ├── Functions/
│   ├── Modulations/
│   ├── Splines/
│   ├── Utilities/
│   └── Visualizations/
│
├── Pipeline/                        # Declarative pipeline definitions
│   └── pipeline.json                # FFSC advanced pipeline
│
├── Tests/vscode/                    # Unit tests
│   ├── Test_PipelineCore.cs         # Pipeline core tests
│   └── Test_AllNodes.cs             # Node integration tests
│
├── config/                          # JSON configuration files
├── docs/                            # Documentation
├── Program.cs                       # Application entry point
├── PicoGKExamples.csproj            # Project file (.NET 9.0, 0 warnings)
└── PicoGK_Examples.sln              # Solution file
```

## Node Registry (22 Nodes)

| Node ID | Name | Input Type | Output Type | Module |
|---------|------|-----------|-------------|--------|
| load_params | Load Engine Parameters | string | EngineParams | Core |
| thermo | Thermodynamic Analysis | ThermoTaskInput | ThermoMap | Physics |
| thickness | Structural Thickness | ThicknessTaskInput | ThicknessMap | Physics |
| cooling_analysis | Cooling Heat Transfer | CoolingTaskInput | CoolingMap | Physics |
| turbopump_design | Turbopump Design | TurbopumpTaskInput | TurbopumpDesign | Turbopump |
| geom_chamber | Chamber Geometry | ThermoTaskInput | Voxels | Geometry |
| geom_nozzle | Nozzle Geometry | ThermoTaskInput | Voxels | Geometry |
| geom_aerospike | Aerospike Geometry | Unit | Voxels | Geometry |
| geom_manifold_ffsc | FFSC Manifold | Unit | Voxels | Geometry |
| geom_manifold_lox | LOX Manifold | Unit | Voxels | Geometry |
| geom_manifold_ch4 | CH4 Manifold | Unit | Voxels | Geometry |
| geom_injectors | Injectors | ThermoTaskInput | Voxels | Geometry |
| geom_turbopump | Turbopump Geometry | ThermoTaskInput | Voxels | Geometry |
| geom_cooling | Cooling Channels | CoolingGeometryInput | Voxels | Geometry |
| geom_pipes | Feed Pipes | Unit | Voxels | Geometry |
| geom_structural | Structural Frame | Unit | Voxels | Geometry |
| geom_supports | Engine Supports | Unit | Voxels | Geometry |
| physics_stress | Stress Field | StressFieldInput | Voxels | Physics |
| physics_cfd | CFD Thermal | CFDInput | Voxels | Physics |
| lattice_dual | Dual-Layer Lattice | LatticeDualInput | Voxels | Geometry |
| lattice_quasi | Quasicrystal Lattice | LatticeQuasiInput | Voxels | Geometry |
| final_assembly | Final Engine Assembly | AssemblyInput | Voxels | Assembly |
| geometryii_shape | GeometryII Shape Generator | ShapeGeneratorInput | Voxels | GeometryII |

## Pipeline Graph

```
load_params (entry)
    ├── thermo
    │       ├── thickness
    │       └── cooling_analysis
    ├── turbopump_design
    ├── geom_chamber
    │       ├── geom_cooling
    │       ├── physics_stress
    │       ├── physics_cfd
    │       └── final_assembly
    ├── geom_nozzle
    │       ├── physics_cfd
    │       └── final_assembly
    ├── geom_aerospike
    │       ├── geom_cooling
    │       ├── physics_stress
    │       ├── physics_cfd
    │       └── final_assembly
    ├── geom_manifold_ffsc
    │       ├── physics_stress
    │       ├── physics_cfd
    │       └── final_assembly
    ├── geom_manifold_lox
    │       └── final_assembly
    ├── geom_manifold_ch4
    │       └── final_assembly
    ├── geom_injectors
    │       └── final_assembly
    ├── geom_turbopump
    │       └── final_assembly
    ├── geom_pipes
    │       └── final_assembly
    ├── geom_structural
    │       └── final_assembly
    ├── geom_supports
    │       └── final_assembly
    ├── physics_stress
    │       ├── lattice_dual
    │       ├── lattice_quasi
    │       └── final_assembly
    ├── lattice_dual
    │       └── final_assembly
    └── lattice_quasi
            └── final_assembly
```

## Key Design Decisions

### .NET Version
- **Target**: .NET 9.0 (required by PicoGK 1.7.7.4)
- **Build**: 0 warnings, 0 errors
- **Note**: .NET 8 is not compatible with PicoGK 1.7.7.4

### Determinism
- All nodes are pure functions (no side effects, no global state)
- EngineParams loaded from JSON
- Physics solvers use fixed iterations
- Geometry uses deterministic voxel sphere placement

### Typing
- All inputs/outputs use C# generics
- Composite inputs use `record` types
- No `object` or `dynamic` types in public APIs

### Modularity
Each module is self-contained and can be packaged as NuGet:
- **PipelineCore**: Core interfaces and execution engine
- **FFSC.EngineSpec**: Engine parameters and calculators
- **FFSC.Geometry**: Geometry generation
- **FFSC.Physics**: Physics analysis
- **FFSC.Turbopump**: Turbopump design
- **FFSC.Assembly**: Engine assembly
- **FFSC.Viewer**: PicoGK viewer integration
- **GeometryII**: Additional shape generation
- **Leap71.ShapeKernel**: LEAP71 geometry primitives

## Usage

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Test
```bash
dotnet run --project FFSC-PicoGK/FFSC_PicoGK.TestRunner/FFSC_PicoGK.TestRunner.csproj
```

### Modify Pipeline
Edit `Pipeline/pipeline.json` to add/remove/reorder nodes.

## Documentation

- `docs/ARCHITECTURE.md` - System architecture
- `docs/PIPELINE.md` - Pipeline execution details
- `docs/MODULES.md` - Module descriptions
- `docs/GRAPH.md` - Graph dependencies
- `docs/PERFORMANCE.md` - Performance optimization guide
- `docs/IMPLEMENTATION.md` - Implementation summary
