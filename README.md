# FFSC Rocket Engine - Modular Pipeline Architecture

A full-flow staged combustion (FFSC) rocket engine design system built on PicoGK with a modular, graph-based execution pipeline.

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
