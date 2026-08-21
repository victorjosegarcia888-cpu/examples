// GeometryNozzleNode.cs
//
// Node wrapper for nozzle geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Nozzle;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryNozzleNode : ITask<ThermoTaskInput, Voxels>
{
    public string Id => "geom_nozzle";
    public string Name => "Nozzle Geometry";

    public Voxels Run(ThermoTaskInput input)
    {
        return Geometry_Nozzle.Create(input.Params);
    }

    public Voxels Execute(ThermoTaskInput input)
    {
        return Run(input);
    }
}
