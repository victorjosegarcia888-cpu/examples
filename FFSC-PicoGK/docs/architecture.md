# Architecture

## System Overview

FFSC_PicoGK is organized in a layered architecture:

```
┌─────────────────────────────────────────────┐
│  Program.cs (Entry Point)                   │
│  Library.Go(voxelSize, FFSCShowcase.Task)   │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  FFSCShowcase_Advanced.cs                   │
│  Version selection, subsystem isolation      │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        ▼           ▼           ▼
┌──────────┐  ┌──────────┐  ┌──────────┐
│ Assembly │  │  Pipeline│  │  Versions│
│ Modular  │  │ Advanced │  │  v03-v06 │
└──────────┘  └──────────┘  └──────────┘
        │           │           │
        └───────────┼───────────┘
                    ▼
┌─────────────────────────────────────────────┐
│  Geometry Subsystem                         │
│  Chamber, Nozzle, Aerospike, Manifolds,     │
│  Injectors, Turbopump, Cooling, Pipes,      │
│  Structural, Supports                       │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Physics Subsystem                          │
│  Thermo, Structural, CFD, Stress, Cooling   │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Models (EngineParams, Materials, Tables)   │
└─────────────────────────────────────────────┘
```

## Task Pattern

All PicoGK tasks follow this pattern:

```csharp
public static class Task_Name
{
    public static Field3D Task()
    {
        // 1. Load/create EngineParams
        // 2. Compute geometry
        // 3. Return Field3D
        return geom;
    }
}
```

## Data Flow

1. **EngineParams** → single source of truth for all parameters
2. **Geometry Tasks** → Field3D outputs (voxel fields)
3. **Physics Tasks** → consume geometry, produce physics fields
4. **Assembly** → combine all fields into final result
5. **Pipeline** → orchestrate end-to-end flow

## Namespace Structure

```
FFSC_PicoGK
  ├── Models
  │   ├── EngineParams
  │   ├── MaterialSpec
  │   ├── BartzCalculator
  │   ├── ThermoTables
  │   └── VdbExporter
  ├── Geometry
  │   ├── Chamber
  │   ├── Nozzle
  │   ├── Aerospike
  │   ├── Manifolds
  │   ├── Injectors
  │   ├── Turbopump
  │   ├── Cooling
  │   ├── Pipes
  │   ├── Structural
  │   └── Supports
  ├── Physics
  │   ├── Thermo
  │   ├── Structural
  │   ├── CFD
  │   ├── Stress
  │   └── Cooling
  ├── EngineFFSC
  │   ├── Turbopump
  │   ├── Tasks
  │   ├── Assembly
  │   ├── Versions
  │   ├── Utils
  │   └── Tests
  ├── Pipeline
  └── Utils
```
