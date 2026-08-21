// TaskInputs.cs
//
// Composite input types for tasks with multiple dependencies.

using PicoGK;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;

namespace PipelineCore;

public record ThermoTaskInput(EngineParams Params);
public record ThicknessTaskInput(EngineParams Params, ThermoMap Thermo);
public record TurbopumpTaskInput(EngineParams Params, double MassFlowOxidizer);
public record CoolingTaskInput(EngineParams Params, ThermoMap Thermo);
public record CoolingGeometryInput(Voxels Chamber, Voxels Spike);
public record StressFieldInput(Voxels Chamber, Voxels Spike, Voxels Manifold);
public record CFDInput(Voxels Chamber, Voxels Nozzle, Voxels Spike, Voxels Manifold);
public record LatticeDualInput(Voxels StressField, double HighThreshold, double LowThreshold, double HighRadius, double LowRadius);
public record LatticeQuasiInput(Voxels StressField, double Scale, double Intensity);
