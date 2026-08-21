# FFSC Rocket Engine - Modular Pipeline Architecture (Noyron/Powder/Leap71)

## Overview

This repository implements a full-flow staged combustion (FFSC) rocket engine design pipeline using a **Noyron/Powder-style large-node architecture**. Each major engine subsystem is represented as a large graph node containing multiple specialized internal agents. The pipeline is deterministic, reproducible, and executed by a topological scheduler.

## Repository Structure

```
/workspaces/examples/
├── PipelineCore/                                    # Core pipeline infrastructure
│   ├── ITask.cs                                     # Generic task interface
│   ├── Node.cs                                      # Graph node definition
│   ├── Graph.cs                                     # Directed acyclic graph (DAG)
│   ├── Scheduler.cs                                 # Topological execution engine
│   ├── TaskRegistry.cs                              # Task registration and lookup
│   ├── PipelineLoader.cs                            # JSON pipeline loader
│   ├── TaskInputs.cs                                # Composite input types
│   ├── Exceptions.cs                                # Custom exceptions
│   └── Interfaces/                                  # Core contracts
│       ├── IEngineSpec.cs                           # Engine specification
│       ├── IGeometry3D.cs                           # 3D geometry generation
│       ├── ILattice3D.cs                            # Lattice generation
│       ├── ICoolingField.cs                         # Cooling analysis
│       ├── IField3D.cs                              # 3D field analysis
│       └── IEngineFFSC.cs                           # Engine assembly
│
├── FFSC-PicoGK/                                     # Primary FFSC implementation
│   ├── Agents/                                      # Noyron internal agents (14 agents)
│   │   ├── IAgent.cs                                # Agent interface
│   │   ├── GeometryAgent.cs                         # Geometry generation agent
│   │   ├── CoolingAgent.cs                          # Cooling channels agent
│   │   ├── LatticeAgent.cs                          # Lattice generation agent
│   │   ├── PhysicsAgent.cs                          # Physics fields agent
│   │   ├── ValidationAgent.cs                       # Validation agent
│   │   ├── FlowAgent.cs                             # Flow analysis agent
│   │   ├── ImpellerAgent.cs                         # Impeller design agent
│   │   ├── ShaftAgent.cs                            # Shaft design agent
│   │   ├── CFDProxyAgent.cs                         # CFD proxy agent
│   │   ├── ThermalGradientAgent.cs                  # Thermal gradient agent
│   │   ├── AssemblyAgent.cs                         # Assembly agent
│   │   ├── InterfaceAgent.cs                        # Interface agent
│   │   └── VisualizationAgent.cs                    # Visualization agent
│   │
│   ├── Nodes/Noyron/                                # Noyron large nodes (9 nodes)
│   │   ├── Nodo_CamaraCombustion.cs                 # Chamber node
│   │   ├── Nodo_PreBurner.cs                        # Preburner node
│   │   ├── Nodo_ManifoldPrincipal.cs                # Manifold node
│   │   ├── Nodo_Turbobomba.cs                       # Turbopump node
│   │   ├── Nodo_CamposFisicos.cs                    # Physics fields node
│   │   ├── Nodo_CoolingRegenerativo.cs              # Cooling node
│   │   ├── Nodo_LatticeAdaptativo.cs                # Lattice node
│   │   ├── Nodo_AssemblyFFSC.cs                     # Assembly node
│   │   └── Nodo_VisualizarMotor.cs                  # Visualization node
│   │
│   ├── Pipeline/                                    # Pipeline definitions
│   │   ├── PipelineBuilder.cs                       # Graph builder + registry
│   │   └── FFSC_Pipeline_Advanced.cs                # Legacy advanced pipeline
│   │
│   ├── EngineFFSC/                                  # Legacy/modular engine code
│   │   ├── Models/                                  # Data models
│   │   ├── Geometry/                                # Geometry primitives
│   │   ├── Physics/                                 # Physics analysis
│   │   ├── Turbopump/                               # Turbopump design
│   │   ├── Assembly/                                # Modular assembly
│   │   ├── Versions/                                # Version wrappers (v03-v06)
│   │   ├── Utils/                                   # Configuration loaders
│   │   └── Tasks/                                   # Task wrappers
│   │
│   ├── config/                                      # JSON configuration files
│   └── FFSC_PicoGK.TestRunner/                      # Test project
│
├── EngineFFSC/                                      # Legacy v06/v07 implementation
├── ShapeKernel/                                     # LEAP71 ShapeKernel (reference)
├── GeometryII/                                      # GeometryII integration
├── Pipeline/                                        # Declarative pipeline definitions
│   └── pipeline.json                                # Canonical Noyron pipeline
├── Tests/vscode/                                    # Unit tests
├── docs/                                            # Documentation
├── Program.cs                                       # Application entry point
├── PicoGKExamples.csproj                            # Project file
└── PicoGK_Examples.sln                              # Solution file
```

## Module Classification

### Nodes (ITask Implementations)

Noyron large nodes with internal agents:

| Node ID | Name | Input | Output | Agents |
|---------|------|-------|--------|--------|
| CamaraCombustion | Nodo Cámara de Combustión | `Unit` | `Voxels` | Geometry, Cooling, Lattice, Physics, Validation |
| PreBurner | Nodo PreBurner | `Unit` | `Voxels` | Geometry, Cooling, Lattice, Physics |
| ManifoldPrincipal | Nodo Manifold Principal | `Unit` | `Voxels` | Geometry, Flow, Validation |
| Turbobomba | Nodo Turbobomba | `Unit` | `Voxels` | Geometry, Impeller, Shaft, Cooling, Physics |
| CamposFisicos | Nodo Campos Físicos | `CamposFisicosInput` | `Voxels` | Physics, CFDProxy |
| CoolingRegenerativo | Nodo Cooling Regenerativo | `CoolingRegenerativoInput` | `Voxels` | Cooling, Physics, Flow |
| LatticeAdaptativo | Nodo Lattice Adaptativo | `LatticeAdaptativoInput` | `Voxels` | Lattice, ThermalGradient |
| AssemblyFFSC | Nodo Assembly FFSC | `AssemblyFFSCInput` | `Voxels` | Assembly, Interface, Validation |
| VisualizarMotor | Nodo Visualizar Motor | `VisualizarMotorInput` | `Voxels` | Visualization |

Legacy fine-grained nodes (also registered):

| Node ID | Input | Output | Module |
|---------|-------|--------|--------|
| load_params | `string` | `EngineParams` | Core |
| thermo | `EngineParams` | `ThermoMap` | Physics |
| thickness | `EngineParams, ThermoMap` | `ThicknessMap` | Physics |
| turbopump_design | `EngineParams, double` | `TurbopumpDesign` | Turbopump |
| geom_chamber | `EngineParams` | `Voxels` | Geometry |
| geom_nozzle | `EngineParams` | `Voxels` | Geometry |
| geom_aerospike | `Unit` | `Voxels` | Geometry |
| geom_manifold_ffsc | `Unit` | `Voxels` | Geometry |
| geom_manifold_lox | `Unit` | `Voxels` | Geometry |
| geom_manifold_ch4 | `Unit` | `Voxels` | Geometry |
| geom_injectors | `EngineParams` | `Voxels` | Geometry |
| geom_turbopump | `EngineParams` | `Voxels` | Geometry |
| geom_cooling | `Voxels, Voxels` | `Voxels` | Geometry |
| geom_pipes | `Unit` | `Voxels` | Geometry |
| geom_structural | `Unit` | `Voxels` | Geometry |
| geom_supports | `Unit` | `Voxels` | Geometry |
| physics_stress | `Voxels, Voxels, Voxels` | `Voxels` | Physics |
| physics_cfd | `Voxels, Voxels, Voxels, Voxels` | `Voxels` | Physics |
| lattice_dual | `Voxels, double, double, double, double` | `Voxels` | Geometry |
| lattice_quasi | `Voxels, double, double` | `Voxels` | Geometry |
| final_assembly | `AssemblyInput` | `Voxels` | Assembly |

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
| NoyronNodes | FFSC-PicoGK/Nodes/Noyron/ | Large engine nodes with internal agents | Yes |
| NoyronAgents | FFSC-PicoGK/Agents/ | Specialized internal agents | Yes |

### Resources (Data Files)

| Resource | Path | Purpose |
|----------|------|---------|
| Engine Parameters | config/engine_params.json | Master engine specification |
| Geometry Config | config/geometry_config.json | Per-component geometry params |
| Cooling Config | config/cooling_config.json | Cooling channel specifications |
| Lattice Config | config/lattice_config.json | Lattice generation params |
| Turbopump Config | config/turbopump_config.json | Turbopump design params |
| Pipeline Definition | Pipeline/pipeline.json | Canonical Noyron pipeline graph |

## Noyron/Powder Design Pattern

Each large node in the Noyron architecture represents a complete piece of machinery with multiple internal agents:

```
Nodo_Turbobomba (example)
├── GeometryAgent       → Generates base geometry
├── ImpellerAgent       → Designs impeller (r1, r2, h, U2, Cu2, ω)
├── ShaftAgent          → Designs shaft
├── CoolingAgent        → Generates cooling channels
├── PhysicsAgent        → Computes stress/thermal fields
└── ValidationAgent     → Validates assembly
```

This pattern is applied to all 9 major engine subsystems:
- **CamaraCombustion**: 5 agents (Geometry, Cooling, Lattice, Physics, Validation)
- **PreBurner**: 4 agents (Geometry, Cooling, Lattice, Physics)
- **ManifoldPrincipal**: 3 agents (Geometry, Flow, Validation)
- **Turbobomba**: 5 agents (Geometry, Impeller, Shaft, Cooling, Physics)
- **CamposFisicos**: 2 agents (Physics, CFDProxy)
- **CoolingRegenerativo**: 3 agents (Cooling, Physics, Flow)
- **LatticeAdaptativo**: 2 agents (Lattice, ThermalGradient)
- **AssemblyFFSC**: 3 agents (Assembly, Interface, Validation)
- **VisualizarMotor**: 1 agent (Visualization)
