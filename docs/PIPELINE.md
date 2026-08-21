# Pipeline Documentation (Noyron/Powder Architecture)

## Overview

The FFSC rocket engine design pipeline is a directed acyclic graph (DAG) with **9 large Noyron-style nodes**. Each node represents a complete engine subsystem and contains multiple specialized internal agents. Nodes execute in topological order, passing typed `Voxels` outputs to dependent nodes.

## Canonical Pipeline

**File**: `Pipeline/pipeline.json`

## Graph Structure (Noyron Large Nodes)

```
Geometry_Chamber      (CamaraCombustion)
Geometry_PreBurner    (PreBurner)
Geometry_Manifold     (ManifoldPrincipal)
TurbopumpDesign       (Turbobomba)
    │
    ├──► PhysicsFields    (CamposFisicos)
    │       ├──► CoolingField        (CoolingRegenerativo) ──┐
    │       └──► GenerateLattice     (LatticeAdaptativo) ───┘
    │                                               │
    ├──► AssemblyFFSC ◄──────────────────────────────┘
    │       ▲
    └───────┘
            │
            ▼
    VisualizeFFSC        (VisualizarMotor)
```

## Node Specifications

### 1. CamaraCombustion (Geometry_Chamber)
- **Agents**: GeometryAgent, CoolingAgent, LatticeAgent, PhysicsAgent, ValidationAgent
- **Input**: `Unit` (standalone)
- **Output**: `Voxels` (complete chamber with throat, convergent, divergent, internal manifold)
- **Dependencies**: None

### 2. PreBurner (Geometry_PreBurner)
- **Agents**: GeometryAgent, CoolingAgent, LatticeAgent, PhysicsAgent
- **Input**: `Unit` (standalone)
- **Output**: `Voxels` (preburner with internal channels and adaptive lattice)
- **Dependencies**: None

### 3. ManifoldPrincipal (Geometry_Manifold)
- **Agents**: GeometryAgent, FlowAgent, ValidationAgent
- **Input**: `Unit` (standalone)
- **Output**: `Voxels` (main manifold)
- **Dependencies**: None

### 4. Turbobomba (TurbopumpDesign)
- **Agents**: GeometryAgent, ImpellerAgent, ShaftAgent, CoolingAgent, PhysicsAgent
- **Input**: `Unit` (standalone)
- **Output**: `Voxels` (complete turbopump with r1, r2, h, U2, Cu2, ω)
- **Dependencies**: None

### 5. CamposFisicos (PhysicsFields)
- **Agents**: PhysicsAgent, CFDProxyAgent
- **Input**: `CamposFisicosInput` (Chamber, PreBurner, Manifold, Turbopump)
- **Output**: `Voxels` (thermal, pressure, vibration, flow fields)
- **Dependencies**: CamaraCombustion, PreBurner, ManifoldPrincipal, Turbobomba
- **Physics Feedback**: Generates fields that feed into CoolingRegenerativo and LatticeAdaptativo

### 6. CoolingRegenerativo (CoolingField)
- **Agents**: CoolingAgent, PhysicsAgent, FlowAgent
- **Input**: `CoolingRegenerativoInput` (Chamber, PreBurner, PhysicsFields)
- **Output**: `Voxels` (volumetric internal cooling channels)
- **Dependencies**: CamaraCombustion, PreBurner, CamposFisicos

### 7. LatticeAdaptativo (GenerateLattice)
- **Agents**: LatticeAgent, ThermalGradientAgent
- **Input**: `LatticeAdaptativoInput` (Chamber, PreBurner, PhysicsFields)
- **Output**: `Voxels` (gyroid/quasicrystal lattice based on thermal zones)
- **Dependencies**: CamaraCombustion, PreBurner, CamposFisicos

### 8. AssemblyFFSC (AssemblyFFSC)
- **Agents**: AssemblyAgent, InterfaceAgent, ValidationAgent
- **Input**: `AssemblyFFSCInput` (all 7 previous nodes)
- **Output**: `Voxels` (complete EngineFFSC volumetric assembly)
- **Dependencies**: All previous nodes

### 9. VisualizarMotor (VisualizeFFSC)
- **Agents**: VisualizationAgent
- **Input**: `VisualizarMotorInput` (EngineFFSC)
- **Output**: `Voxels` (Field3D ready for PicoGK viewer)
- **Dependencies**: AssemblyFFSC

## Execution Phases

### Phase 1: Independent Geometry Generation (parallel)
- **CamaraCombustion**: Generates complete chamber geometry
- **PreBurner**: Generates preburner geometry
- **ManifoldPrincipal**: Generates manifold geometry
- **Turbobomba**: Generates complete turbopump geometry

### Phase 2: Physics Analysis
- **CamposFisicos**: Computes thermal, pressure, vibration, and flow fields from all Phase 1 geometries

### Phase 3: Subsystem Refinement (parallel)
- **CoolingRegenerativo**: Generates volumetric cooling channels using physics feedback
- **LatticeAdaptativo**: Generates adaptive lattice using physics feedback

### Phase 4: Final Assembly
- **AssemblyFFSC**: Assembles all subsystems into complete EngineFFSC

### Phase 5: Visualization
- **VisualizarMotor**: Converts to PicoGK viewer format

## Dependency Rules

1. **No Implicit Dependencies**: All dependencies are explicit in `pipeline.json`
2. **Strict Typing**: Each node has strictly typed input/output records
3. **Determinism**: All nodes are pure functions - same input always produces same output
4. **No Cycles**: The graph is a DAG; cycle detection is enforced by the scheduler
5. **Entry Points**: 4 standalone nodes (CamaraCombustion, PreBurner, ManifoldPrincipal, Turbobomba)
6. **Final Output**: VisualizarMotor is the designated output node

## Input Records (Typed)

```csharp
public record CamposFisicosInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels Manifold,
    Voxels Turbopump);

public record CoolingRegenerativoInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels PhysicsFields);

public record LatticeAdaptativoInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels PhysicsFields);

public record AssemblyFFSCInput(
    Voxels Chamber,
    Voxels PreBurner,
    Voxels Manifold,
    Voxels Turbopump,
    Voxels Cooling,
    Voxels Lattice,
    Voxels Physics);

public record VisualizarMotorInput(Voxels Engine);
```

## Reproducibility

All nodes are deterministic because:
- Geometry uses deterministic voxel sphere placement
- Physics calculations use fixed iterative solvers
- No system time, random numbers, or external state is used
- All inputs are immutable records
- Same inputs always produce identical Voxels
