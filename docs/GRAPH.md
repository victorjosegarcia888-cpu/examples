# Graph Dependencies

## Module Dependency Graph

```
PipelineCore
    ├── EngineSpec
    ├── Geometry
    │       ├── EngineSpec
    │       ├── Turbopump (for turbopump geometry)
    │       └── ShapeKernel (optional, for advanced primitives)
    ├── Physics
    │       ├── EngineSpec
    │       ├── Geometry (for stress/CFD inputs)
    │       └── Turbopump (for pump design)
    ├── Turbopump
    │       ├── EngineSpec
    │       └── Geometry (for voxel generation)
    ├── Assembly
    │       ├── Geometry
    │       ├── Physics
    │       ├── Turbopump
    │       └── EngineSpec
    └── Viewer
            ├── Assembly
            └── PipelineCore
```

## Node Dependency Matrix

| Node | Dependencies | Provides To |
|------|-------------|-------------|
| load_params | - | thermo, thickness, turbopump_design, geom_chamber, geom_nozzle, geom_injectors, geom_turbopump, lattice_dual |
| thermo | load_params | thickness, cooling_analysis |
| thickness | load_params, thermo | - |
| turbopump_design | load_params | - |
| geom_chamber | load_params | geom_cooling, physics_stress, physics_cfd, final_assembly |
| geom_nozzle | load_params | physics_cfd, final_assembly |
| geom_aerospike | - | geom_cooling, physics_stress, physics_cfd, final_assembly |
| geom_manifold_ffsc | - | physics_stress, physics_cfd, final_assembly |
| geom_manifold_lox | - | final_assembly |
| geom_manifold_ch4 | - | final_assembly |
| geom_injectors | load_params | final_assembly |
| geom_turbopump | load_params | final_assembly |
| geom_cooling | geom_chamber, geom_aerospike | final_assembly |
| geom_pipes | - | final_assembly |
| geom_structural | - | final_assembly |
| geom_supports | - | final_assembly |
| physics_stress | geom_chamber, geom_aerospike, geom_manifold_ffsc | lattice_dual, lattice_quasi, final_assembly |
| physics_cfd | geom_chamber, geom_nozzle, geom_aerospike, geom_manifold_ffsc | final_assembly |
| cooling_analysis | load_params, thermo | - |
| lattice_dual | physics_stress, load_params | final_assembly |
| lattice_quasi | physics_stress | final_assembly |
| final_assembly | 16 geometry/physics nodes | - |

## Data Type Flow

```
EngineParams (IEngineSpec)
    ├── ThermoMap (IThermoField)
    │       ├── ThicknessMap
    │       ├── CoolingMap
    │       └── ThicknessMap
    ├── Voxels (IGeometry3D)
    │       ├── StressField (IField3D)
    │       │       ├── Lattice_DualLayer (ILattice3D)
    │       │       └── Lattice_Quasicrystal (ILattice3D)
    │       └── CFDTask (IField3D)
    └── TurbopumpDesign
            └── Voxels (IGeometry3D)

All Voxels → AssemblyNode → Final Voxels → Viewer
```

## Determinism Guarantees

1. **No Random State**: All tasks use deterministic algorithms
2. **Fixed Seeds**: No random number generation
3. **Pure Functions**: Tasks have no side effects
4. **Immutable Inputs**: Inputs are read-only records/structs
5. **Reproducible Outputs**: Same inputs always produce identical Voxels
