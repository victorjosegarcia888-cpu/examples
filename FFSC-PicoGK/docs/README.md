# FFSC_PicoGK
## Full-Flow Staged Combustion Rocket Engine Design with PicoGK

FFSC_PicoGK is a professional-grade parametric design environment for Full-Flow Staged Combustion (FFSC) rocket engines, built on the PicoGK volumetric modeling framework.

## Features

- **Complete Engine Geometry**: Combustion chamber, Rao-optimized nozzle, aerospike, injectors, turbopump, manifolds, cooling channels, structural elements
- **Physics Integration**: Thermodynamic analysis (TAD, Bartz correlation), stress field computation, CFD simplification, regenerative cooling
- **Adaptive Lattice**: Dual-layer and quasicrystal lattices driven by stress/thermal fields
- **Modular Assembly**: Enable/disable subsystems, switch between engine versions (v03-v06)
- **Pipeline Automation**: End-to-end design flow from parameters to geometry to physics

## Getting Started

```bash
cd FFSC-PicoGK
dotnet build
dotnet run
```

## Project Structure

```
FFSC-PicoGK/
  EngineFFSC/
    Geometry/       - All geometric primitives
    Physics/        - Thermodynamics, stress, CFD, cooling
    Models/         - EngineParams, materials, calculators
    Tasks/          - PicoGK-compatible task patterns
    Turbopump/      - Turbopump parametric design
    Pipeline/       - End-to-end automation
    Assembly/       - Modular engine assembly
    Versions/       - v03, v04, v05, v06 configurations
    Utils/          - JSON loaders, exporters
    Tests/          - Unit tests
  Pipeline/         - Main pipeline entry point
  config/           - JSON configuration files
  docs/             - Technical documentation
```

## Engine Versions

| Version | Description |
|---------|-------------|
| v03 | Multi-objective: chamber + aerospike + manifold + primary cooling |
| v04 | Redundant: + secondary cooling + manifold cooling + injectors + turbopump |
| v05 | Adaptive: + full manifold + preburner + turbopump + turbine + pipes |
| v06 | Complete: + nozzle + stress fields + CFD + all subsystems |

## Configuration

Edit `config/engine_params.json` to customize the engine:

```json
{
  "Thrust": 2500000.0,
  "ChamberPressure_bar": 350.0,
  "ExpansionRatio": 45.0,
  "Lstar": 1.2,
  "MixtureRatio": 3.6
}
```

## Theory References

- UC3M Rocket Propulsion PDFs
- "Design of Liquid Propellant Rocket Engines" (Huzel & Huang)
- Bartz Heat Transfer Correlation
- Rao Nozzle Contour Optimization
- Euler Turbomachinery Equations
