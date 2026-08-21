# FFSC_PicoGK - Agent Instructions

## Project Overview

This is a professional Full-Flow Staged Combustion (FFSC) rocket engine design project using PicoGK volumetric modeling framework.

## Key Conventions

### API
- Use `PicoGK.Voxels` (not `Field3D`) for volumetric fields
- Create shapes with `Voxels.voxSphere(Vector3 center, float radius)`
- Combine with `+` operator (union), `-` (subtract), `&` (intersect)
- Add to viewer with `Library.oViewer().Add(voxels)`
- Task methods must return `void` for `Library.Go` compatibility

### Namespaces
- All code uses `FFSC_PicoGK` root namespace
- Sub-namespaces: Models, Geometry, Physics, Tasks, Pipeline, Utils, Tests

### Units
- SI units throughout (meters, kg, seconds, Pascals, Kelvin)
- Voxel size: 0.5mm (passed to Library.Go)

### Geometry Pattern
```csharp
Voxels shape = Voxels.voxSphere(new Vector3(x, y, z), radius);
shape += Voxels.voxSphere(new Vector3(x2, y2, z2), radius2);
```

## Directory Structure
```
EngineFFSC/
  Geometry/     - All geometric primitives (Voxels-based)
  Physics/      - Thermodynamics, stress, CFD, cooling
  Models/       - EngineParams, materials, calculators
  Tasks/        - PicoGK task patterns (void return)
  Turbopump/    - Parametric turbopump design
  Pipeline/     - End-to-end automation
  Assembly/     - Modular engine assembly
  Versions/     - v03-v06 configurations
  Utils/        - JSON loaders, exporters
  Tests/        - Unit tests
Pipeline/       - Main pipeline entry
config/         - JSON configuration files
docs/           - Technical documentation
```

## Build & Run
```bash
dotnet build PicoGKExamples.csproj
dotnet run --project PicoGKExamples.csproj
```
