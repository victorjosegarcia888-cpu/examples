# Pipeline Graph Documentation

## Overview

The FFSC rocket engine design pipeline is a directed acyclic graph (DAG) where each node represents a computational task. Nodes execute in topological order, passing typed outputs to dependent nodes.

## Graph Structure

```
load_params (entry)
    ├── thermo
    │       └── thickness
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

## Execution Phases

### Phase 1: Parameter Loading
- **load_params**: Loads engine specification from JSON

### Phase 2: Physics Analysis
- **thermo**: Thermodynamic state computation (TAD, Tg(z), Bartz hg(z))
- **thickness**: Structural thickness from Barlow's formula + thermal margin
- **cooling_analysis**: Regenerative cooling heat transfer
- **turbopump_design**: Centrifugal pump parametric design

### Phase 3: Geometry Generation
- **geom_chamber**: Spherical-cylindrical combustion chamber
- **geom_nozzle**: Rao-optimized parabolic nozzle
- **geom_aerospike**: Linear aerospike with toroidal base
- **geom_manifold_ffsc**: FFSC preburner/return manifold
- **geom_manifold_lox**: LOX toroidal manifold
- **geom_manifold_ch4**: CH4 spiral manifold
- **geom_injectors**: Coaxial injector plate
- **geom_turbopump**: Centrifugal turbopump geometry
- **geom_cooling**: Primary + secondary helical cooling channels
- **geom_pipes**: High-pressure LOX/CH4 feed pipes
- **geom_structural**: Thrust frame and gimbal mounts
- **geom_supports**: Engine support struts

### Phase 4: Physics Fields
- **physics_stress**: Volumetric stress field (dynamic)
- **physics_cfd**: Simplified CFD thermal field

### Phase 5: Lattice Generation
- **lattice_dual**: Stress-adaptive dual-layer Gyroid lattice
- **lattice_quasi**: Quasicrystal lattice pattern

### Phase 6: Final Assembly
- **final_assembly**: Combines all subsystems into final engine Voxels

## Dependency Rules

1. **Data Flow**: Each node receives typed inputs from its dependencies
2. **Determinism**: All tasks are pure functions - same input always produces same output
3. **No Cycles**: The graph is a DAG; cycle detection is enforced by the scheduler
4. **Entry Points**: Nodes with no dependencies are entry points (load_params, geom_aerospike, etc.)
5. **Final Output**: final_assembly is the designated output node

## Reproducibility

All nodes are deterministic because:
- EngineParams are loaded from JSON (no randomness)
- Physics calculations use fixed iterative solvers
- Geometry uses deterministic voxel sphere placement
- No system time, random numbers, or external state is used
