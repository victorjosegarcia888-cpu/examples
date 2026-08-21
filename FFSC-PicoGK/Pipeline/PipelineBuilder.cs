// PipelineBuilder.cs
//
// Builds the execution graph from the pipeline definition.
// Sets up InputFactory for complex multi-dependency nodes
// and registers all tasks in the TaskRegistry.

using System;
using System.IO;
using PicoGK;
using PipelineCore;
using FFSC_PicoGK.Models;
using FFSC_PicoGK.Physics.Thermo;
using FFSC_PicoGK.EngineFFSC.Turbopump;
using FFSC_PicoGK.Pipeline.Nodes;
using FFSC_PicoGK.Nodes.Noyron;

namespace FFSC_PicoGK.Pipeline;

public static class PipelineBuilder
{
    public static Graph BuildPipeline(string pipelineJsonPath, TaskRegistry registry)
    {
        Graph graph = PipelineLoader.Load(pipelineJsonPath);

        foreach (var nodeId in graph.NodeIds)
        {
            if (!graph.TryGetNode(nodeId, out Node? node) || node is null)
                continue;

            node.InputFactory = node.Id switch
            {
                "thermo" => ctx => new ThermoTaskInput(ctx.Get<EngineParams>("load_params")!),
                "thickness" => ctx => new ThicknessTaskInput(
                    ctx.Get<EngineParams>("load_params")!,
                    ctx.Get<Physics.Thermo.ThermoMap>("thermo")!),
                "turbopump_design" => ctx => new TurbopumpTaskInput(
                    ctx.Get<EngineParams>("load_params")!,
                    320.0),
                "geom_chamber" => ctx => new ThermoTaskInput(ctx.Get<EngineParams>("load_params")!),
                "geom_nozzle" => ctx => new ThermoTaskInput(ctx.Get<EngineParams>("load_params")!),
                "geom_injectors" => ctx => new ThermoTaskInput(ctx.Get<EngineParams>("load_params")!),
                "geom_turbopump" => ctx => new ThermoTaskInput(ctx.Get<EngineParams>("load_params")!),
                "geom_cooling" => ctx => new CoolingGeometryInput(
                    ctx.Get<Voxels>("geom_chamber")!,
                    ctx.Get<Voxels>("geom_aerospike")!),
                "physics_stress" => ctx => new StressFieldInput(
                    ctx.Get<Voxels>("geom_chamber")!,
                    ctx.Get<Voxels>("geom_aerospike")!,
                    ctx.Get<Voxels>("geom_manifold_ffsc")!),
                "physics_cfd" => ctx => new CFDInput(
                    ctx.Get<Voxels>("geom_chamber")!,
                    ctx.Get<Voxels>("geom_nozzle")!,
                    ctx.Get<Voxels>("geom_aerospike")!,
                    ctx.Get<Voxels>("geom_manifold_ffsc")!),
                "lattice_dual" => ctx => new LatticeDualInput(
                    ctx.Get<Voxels>("physics_stress")!,
                    0.6,
                    0.3,
                    0.015,
                    0.008),
                "lattice_quasi" => ctx => new LatticeQuasiInput(
                    ctx.Get<Voxels>("physics_stress")!,
                    0.3,
                    0.5),
                "final_assembly" => ctx => new AssemblyInput(
                    ctx.Get<Voxels>("geom_chamber")!,
                    ctx.Get<Voxels>("geom_nozzle")!,
                    ctx.Get<Voxels>("geom_aerospike")!,
                    ctx.Get<Voxels>("geom_manifold_ffsc")!,
                    ctx.Get<Voxels>("geom_manifold_lox")!,
                    ctx.Get<Voxels>("geom_manifold_ch4")!,
                    ctx.Get<Voxels>("geom_injectors")!,
                    ctx.Get<Voxels>("geom_turbopump")!,
                    ctx.Get<Voxels>("geom_cooling")!,
                    ctx.Get<Voxels>("geom_pipes")!,
                    ctx.Get<Voxels>("geom_structural")!,
                    ctx.Get<Voxels>("geom_supports")!,
                    ctx.Get<Voxels>("physics_stress")!,
                    ctx.Get<Voxels>("physics_cfd")!,
                    ctx.Get<Voxels>("lattice_dual")!,
                    ctx.Get<Voxels>("lattice_quasi")!),
                "CamaraCombustion" => ctx => Unit.Value,
                "PreBurner" => ctx => Unit.Value,
                "ManifoldPrincipal" => ctx => Unit.Value,
                "Turbobomba" => ctx => Unit.Value,
                "CamposFisicos" => ctx => new CamposFisicosInput(
                    ctx.Get<Voxels>("CamaraCombustion")!,
                    ctx.Get<Voxels>("PreBurner")!,
                    ctx.Get<Voxels>("ManifoldPrincipal")!,
                    ctx.Get<Voxels>("Turbobomba")!),
                "CoolingRegenerativo" => ctx => new CoolingRegenerativoInput(
                    ctx.Get<Voxels>("CamaraCombustion")!,
                    ctx.Get<Voxels>("PreBurner")!,
                    ctx.Get<Voxels>("CamposFisicos")!),
                "LatticeAdaptativo" => ctx => new LatticeAdaptativoInput(
                    ctx.Get<Voxels>("CamaraCombustion")!,
                    ctx.Get<Voxels>("PreBurner")!,
                    ctx.Get<Voxels>("CamposFisicos")!),
                "OrganicManifold" => ctx => new OrganicManifoldInput(
                    ctx.Get<Voxels>("ManifoldPrincipal")!,
                    ctx.Get<Voxels>("CamposFisicos")!),
                "FractalPostProcess" => ctx => new FractalPostProcessInput(
                    ctx.Get<Voxels>("CamaraCombustion")!,
                    ctx.Get<Voxels>("PreBurner")!,
                    ctx.Get<Voxels>("ManifoldPrincipal")!,
                    ctx.Get<Voxels>("Turbobomba")!,
                    ctx.Get<Voxels>("CoolingRegenerativo")!,
                    ctx.Get<Voxels>("LatticeAdaptativo")!,
                    ctx.Get<Voxels>("CamposFisicos")!,
                    ctx.Get<Voxels>("OrganicManifold")!),
                "AssemblyFFSC" => ctx => new AssemblyFFSCInput(
                    ctx.Get<Voxels>("CamaraCombustion")!,
                    ctx.Get<Voxels>("PreBurner")!,
                    ctx.Get<Voxels>("ManifoldPrincipal")!,
                    ctx.Get<Voxels>("Turbobomba")!,
                    ctx.Get<Voxels>("CoolingRegenerativo")!,
                    ctx.Get<Voxels>("LatticeAdaptativo")!,
                    ctx.Get<Voxels>("CamposFisicos")!,
                    ctx.Get<Voxels>("FractalPostProcess")!),
                "VisualizarMotor" => ctx => new VisualizarMotorInput(
                    ctx.Get<Voxels>("AssemblyFFSC")!),
                _ => null
            };

            if (node.TaskType != null)
            {
                Type? taskType = Type.GetType(node.TaskType);
                if (taskType != null && !registry.Contains(node.Id))
                {
                    object taskInstance = Activator.CreateInstance(taskType)!;
                    registry.RegisterInstance(node.Id, taskInstance);
                }
            }
        }

        return graph;
    }

    public static void RegisterAllTasks(TaskRegistry registry)
    {
        registry.Register("load_params", new LoadParamsNode());
        registry.Register("thermo", new ThermoNode());
        registry.Register("thickness", new ThicknessNode());
        registry.Register("turbopump_design", new TurbopumpDesignNode());
        registry.Register("geom_chamber", new GeometryChamberNode());
        registry.Register("geom_nozzle", new GeometryNozzleNode());
        registry.Register("geom_aerospike", new GeometryAerospikeNode());
        registry.Register("geom_manifold_ffsc", new GeometryManifoldFFSCNode());
        registry.Register("geom_manifold_lox", new GeometryManifoldLOXNode());
        registry.Register("geom_manifold_ch4", new GeometryManifoldCH4Node());
        registry.Register("geom_injectors", new GeometryInjectorsNode());
        registry.Register("geom_turbopump", new GeometryTurbopumpNode());
        registry.Register("geom_cooling", new GeometryCoolingNode());
        registry.Register("geom_pipes", new GeometryPipesNode());
        registry.Register("geom_structural", new GeometryStructuralNode());
        registry.Register("geom_supports", new GeometrySupportsNode());
        registry.Register("physics_stress", new StressFieldNode());
        registry.Register("physics_cfd", new CFDNode());
        registry.Register("lattice_dual", new LatticeDualNode());
        registry.Register("lattice_quasi", new LatticeQuasiNode());
        registry.Register("final_assembly", new AssemblyNode());
        registry.Register("CamaraCombustion", new Nodo_CamaraCombustion());
        registry.Register("PreBurner", new Nodo_PreBurner());
        registry.Register("ManifoldPrincipal", new Nodo_ManifoldPrincipal());
        registry.Register("Turbobomba", new Nodo_Turbobomba());
        registry.Register("CamposFisicos", new Nodo_CamposFisicos());
        registry.Register("CoolingRegenerativo", new Nodo_CoolingRegenerativo());
        registry.Register("LatticeAdaptativo", new Nodo_LatticeAdaptativo());
        registry.Register("OrganicManifold", new Nodo_OrganicManifold());
        registry.Register("FractalPostProcess", new Nodo_FractalPostProcess());
        registry.Register("AssemblyFFSC", new Nodo_AssemblyFFSC());
        registry.Register("VisualizarMotor", new Nodo_VisualizarMotor());
    }
}

