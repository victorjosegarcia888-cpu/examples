# FFSC Rocket Engine - Modular Pipeline Architecture

A full-flow staged combustion (FFSC) rocket engine design system built on PicoGK with a modular, graph-based execution pipeline.
openvbd analysis: 
1. academicsoftware:https://www.openvdb.org/documentation/doxygen/namespaceopenvdb_1_1v13__1_1_1tools.html#adcddfc57ef94f93ff3806a45c26ca7d0
2. Topology_level:https://github.com/victorjosegarcia888-cpu/nanodb/blob/main/openvdb/tools/TopologyToLevelSet.h
3. Maps:https://www.openvdb.org/documentation/doxygen/Maps_8h.html
4. AmreX/paraview https://amrex-codes.github.io/amrex/docs_html/EB.html
5. imgui/openGL for graphics/pikogk runtime viewer

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Pipeline Execution Graph                  │
│  ┌──────────┐    ┌──────────┐    ┌──────────────────────┐  │
│  │ load_    │───▶│ thermo   │───▶│ thickness, cooling_  │  │
│  │ params   │    │          │    │ analysis              │  │
│  └────┬─────┘    └────┬─────┘    └──────────────────────┘  │
│       │               │                                     │
│       ▼               ▼                                     ▼
│  ┌──────────┐    ┌──────────┐    ┌──────────────────────┐  │
│  │ turbopump│    │ geometry │    │ physics_stress,       │  │
│  │ _design  │    │ nodes    │    │ physics_cfd           │  │
│  └──────────┘    └────┬─────┘    └──────────┬───────────┘  │
│                       │                     │               │
│                       ▼                     ▼               │
│                ┌──────────────┐    ┌──────────────────┐    │
│                │ lattice_dual │    │ lattice_quasi    │    │
│                │ lattice_quasi│    └────────┬─────────┘    │
│                └──────┬──────┘             │               │
│                       └───────────┬───────┘               │
│                                   ▼                         │
│                        ┌──────────────────┐                 │
│                        │ final_assembly   │                 │
│                        └──────────────────┘                 │
└─────────────────────────────────────────────────────────────┘
```

## Modules

| Module | Purpose | NuGet |
|--------|---------|-------|
| PipelineCore | ITask, Graph, Scheduler, Registry | `PipelineCore` |
| EngineSpec | Engine parameters, materials, calculators | `FFSC.EngineSpec` |
| Geometry | Chamber, nozzle, manifolds, injectors, etc. | `FFSC.Geometry` |
| Physics | Thermo, stress, CFD, cooling, thickness | `FFSC.Physics` |
| Turbopump | Centrifugal pump design | `FFSC.Turbopump` |
| Assembly | Modular engine assembly | `FFSC.Assembly` |
| Viewer | PicoGK runtime viewer integration | `FFSC.Viewer` |

AI instructions: pikogk - c++(openvbd) EB amREX physics//
Geometry
↓
Mesh
↓
SDF
↓
Interpolation
↓
Implicit Function
↓
EB2

IMGUI/openGL visualization/renderization graphics/user interface similar to Java Applet in c++

## Quick Start

```bash
dotnet build
dotnet run
```

This executes the declarative pipeline defined in `Pipeline/pipeline.json` and displays the engine in the PicoGK viewer.

## Running Tests

```bash
dotnet run --project FFSC-PicoGK/FFSC_PicoGK.TestRunner/FFSC_PicoGK.TestRunner.csproj
```

## Pipeline Definition

The pipeline is defined declaratively in `Pipeline/pipeline.json`:

```json
{
  "name": "FFSC Advanced Pipeline",
  "nodes": [
    {
      "id": "load_params",
      "taskType": "FFSC_PicoGK.Pipeline.Nodes.LoadParamsNode",
      "dependsOn": [],
      "input": { "configPath": "config/engine_params.json" },
      "outputKey": "engine_params"
    },
    {
      "id": "thermo",
      "taskType": "FFSC_PicoGK.Pipeline.Nodes.ThermoNode",
      "dependsOn": ["load_params"],
      "input": { "params": { "$ref": "load_params" } },
      "outputKey": "thermo_map"
    }
    // ... additional nodes
  ],
  "outputNodes": ["final_assembly"]
}
```

## Key Features

- **Modular**: Each engine subsystem is an independent ITask node
- **Typed**: All inputs/outputs are strictly typed with C# generics
- **Declarative**: Pipeline structure defined in JSON
- **Deterministic**: Pure functions, no side effects, reproducible
- **Scalable**: Easy to add new nodes or modify dependencies
- **Testable**: Each node can be tested independently

## Documentation

- `docs/ARCHITECTURE.md` - System architecture
- `docs/PIPELINE.md` - Pipeline execution details
- `docs/MODULES.md` - Module descriptions
- `docs/GRAPH.md` - Graph dependencies and data flow

## Requirements

- .NET 9.0 SDK
- PicoGK 1.7.7.4
- LEAP71 ShapeKernel (optional, for advanced geometry)
- link_CFD_ARM: https://amrex-codes.github.io/amrex/docs_html/AmrCore.html
