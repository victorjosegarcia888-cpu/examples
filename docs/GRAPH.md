# Graph Dependencies

## Noyron/Powder Large-Node Graph

```
Nodo_CamaraCombustion (standalone)
Nodo_PreBurner (standalone)
Nodo_ManifoldPrincipal (standalone)
Nodo_Turbobomba (standalone)
    │
    ├──► Nodo_CamposFisicos
    │       ├── Nodo_CoolingRegenerativo
    │       │       └──► Nodo_AssemblyFFSC ◄────────────┐
    │       └── Nodo_LatticeAdaptativo                   │
    │               └──► Nodo_AssemblyFFSC ◄────────────┘
    │
    ├──► Nodo_AssemblyFFSC
    │
    └──► Nodo_AssemblyFFSC
            │
            ▼
    Nodo_VisualizarMotor (final output)
```

## Node Dependency Matrix (Noyron Architecture)

| Nodo | Agentes Internos | Dependencies | Provides To | Input Tipado | Output Tipado |
|------|-----------------|--------------|-------------|--------------|---------------|
| CamaraCombustion | Geometry, Cooling, Lattice, Physics, Validation | - | CamposFisicos, CoolingRegenerativo, LatticeAdaptativo, AssemblyFFSC | `Unit` | `Voxels` |
| PreBurner | Geometry, Cooling, Lattice, Physics | - | CamposFisicos, CoolingRegenerativo, LatticeAdaptativo, AssemblyFFSC | `Unit` | `Voxels` |
| ManifoldPrincipal | Geometry, Flow, Validation | - | CamposFisicos, AssemblyFFSC | `Unit` | `Voxels` |
| Turbobomba | Geometry, Impeller, Shaft, Cooling, Physics | - | CamposFisicos, AssemblyFFSC | `Unit` | `Voxels` |
| CamposFisicos | Physics, CFDProxy | CamaraCombustion, PreBurner, ManifoldPrincipal, Turbobomba | CoolingRegenerativo, LatticeAdaptativo, AssemblyFFSC | `CamposFisicosInput` | `Voxels` |
| CoolingRegenerativo | Cooling, Physics, Flow | CamaraCombustion, PreBurner, CamposFisicos | AssemblyFFSC | `CoolingRegenerativoInput` | `Voxels` |
| LatticeAdaptativo | Lattice, ThermalGradient | CamaraCombustion, PreBurner, CamposFisicos | AssemblyFFSC | `LatticeAdaptativoInput` | `Voxels` |
| AssemblyFFSC | Assembly, Interface, Validation | Todos los anteriores | VisualizarMotor | `AssemblyFFSCInput` | `Voxels` |
| VisualizarMotor | Visualization | AssemblyFFSC | - | `VisualizarMotorInput` | `Voxels` |

## Edge List (18 edges)

| Source | Target | Data Flow |
|--------|--------|-----------|
| CamaraCombustion | CamposFisicos | Voxels |
| CamaraCombustion | CoolingRegenerativo | Voxels |
| CamaraCombustion | LatticeAdaptativo | Voxels |
| CamaraCombustion | AssemblyFFSC | Voxels |
| PreBurner | CamposFisicos | Voxels |
| PreBurner | CoolingRegenerativo | Voxels |
| PreBurner | LatticeAdaptativo | Voxels |
| PreBurner | AssemblyFFSC | Voxels |
| ManifoldPrincipal | CamposFisicos | Voxels |
| ManifoldPrincipal | AssemblyFFSC | Voxels |
| Turbobomba | CamposFisicos | Voxels |
| Turbobomba | AssemblyFFSC | Voxels |
| CamposFisicos | CoolingRegenerativo | Voxels |
| CamposFisicos | LatticeAdaptativo | Voxels |
| CamposFisicos | AssemblyFFSC | Voxels |
| CoolingRegenerativo | AssemblyFFSC | Voxels |
| LatticeAdaptativo | AssemblyFFSC | Voxels |
| AssemblyFFSC | VisualizarMotor | Voxels |

## Data Type Flow (Noyron)

```
Unit (entry nodes)
    ├── CamaraCombustion ──┐
    ├── PreBurner ─────────┼──► CamposFisicosInput ──► CamposFisicos ──┐
    ├── ManifoldPrincipal ──┘                                        │
    ├── Turbobomba ───────────────────────────────────────────────────┘
                                                                       │
Unit (entry nodes)                                                     │
    ├── CamaraCombustion ──────────────────────────────────────────────┤
    ├── PreBurner ─────────────────────────────────────────────────────┤
    ├── ManifoldPrincipal ─────────────────────────────────────────────┤
    ├── Turbobomba ────────────────────────────────────────────────────┤
    ├── CoolingRegenerativo ───────────────────────────────────────────┤
    └── LatticeAdaptativo ─────────────────────────────────────────────┘
                                                                       │
                                                                       ▼
                                                               AssemblyFFSCInput
                                                                       │
                                                                       ▼
                                                              VisualizarMotorInput
                                                                       │
                                                                       ▼
                                                            Voxels (EngineFFSC)
```

## Determinism Guarantees

1. **No Random State**: All agents use deterministic algorithms
2. **Pure Functions**: Each node is a pure function `TInput -> VOutput`
3. **Immutable Inputs**: Inputs are readonly records/structs
4. **No Side Effects**: Nodes only produce output, no external mutation
5. **Reproducible Outputs**: Same inputs always produce identical Voxels
6. **Topological Order**: Scheduler respects dependencies exactly
7. **No Global State**: No static mutable state anywhere
