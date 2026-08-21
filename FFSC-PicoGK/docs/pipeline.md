# Pipeline

## FFSC_Pipeline_Advanced

The advanced pipeline orchestrates the complete design flow:

### Phase 1: Parameters
- Load EngineParams from JSON
- Validate parameter ranges

### Phase 2: Thermochemistry
- Compute Tad (adiabatic flame temperature)
- Compute hg(z) using Bartz correlation
- Normalize Qnorm(z)

### Phase 3: Structural
- Compute wall thickness using Barlow's formula
- Apply thermal degradation factor
- Generate ThicknessMap

### Phase 4: Turbopump Design
- Euler equation for head
- Continuity for flow rate
- Blade geometry from velocity triangles

### Phase 5: Geometry Generation
- Combustion chamber (spherical + cylindrical + convergent)
- Nozzle (Rao-optimized contour)
- Aerospike (linear altitude-compensating)
- Manifolds (LOX, CH4, FFSC complete)
- Injectors (coaxial, 32 elements)
- Turbopump (NACA blades, volute, shaft)
- Cooling channels (helical primary/secondary)
- Pipes (high-pressure routing)
- Structural (thrust frame, gimbal, skirt)
- Supports (4 struts, base plate)

### Phase 6: Physics Fields
- Stress field (static/dynamic)
- Thermal field (static/dynamic)
- CFD field (velocity/pressure approximation)

### Phase 7: Lattice
- Dual-layer (stress-driven)
- Quasicrystal (thermal-driven)

### Phase 8: Assembly
- Combine all Field3D objects
- Apply boolean operations
- Export results

## Usage

```csharp
EngineParams p = EngineParamsLoader.Load("config/engine_params.json");
Field3D engine = FFSC_Pipeline_Advanced.Execute(p);
```
