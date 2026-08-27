# EngineFFSC - Modular FFSC Rocket Engine

Modular Full-Flow Staged Combustion (FFSC) rocket engine design system built on PicoGK.

## Structure

```
EngineFFSC/
  Igniters/           - Igniter vector and voxel models
  Preburners/         - ORPB and FRPB preburner models
  Turbopumps/         - Turbopump voxel and vector models
  CombustionChamber/  - Chamber grid, chemistry, nozzle geometry
  Materials/          - Inconel A286 and alloy database
  Geometry/           - Lattice engine, quasicrystals, ShapeKernel extensions
  EngineAssembly/     - FFSC_Engine orchestrator and integrator
  Analysis/           - Task analysis
  Pipeline/           - Pipeline scripts
  Versions/           - v06/v07 configurations
  Tests/              - Unit tests
  Utils/              - Utilities
```

## Modules

| Module | Purpose |
|--------|---------|
| Igniters | Vector and voxel igniter models |
| Preburners | ORPB (oxidizer-rich) and FRPB (fuel-rich) preburners |
| Turbopumps | Voxel/vector turbopump models with cavitation analysis |
| CombustionChamber | Chamber grid, chemistry, nozzle geometry |
| Materials | Inconel A286 properties and alloy database |
| Geometry | Lattice, quasicrystal, and ShapeKernel primitives |
| EngineAssembly | FFSC_Engine orchestrator and EngineIntegrator |

## Build & Run

```bash
dotnet build
dotnet run
```

## Requirements

- .NET 9.0 SDK
- PicoGK 1.7.7.4
- LEAP71 ShapeKernel (optional)
