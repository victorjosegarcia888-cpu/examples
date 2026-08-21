// GeometryTurbopumpNode.cs
//
// Node wrapper for turbopump geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Turbopump;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryTurbopumpNode : ITask<ThermoTaskInput, Voxels>
{
    public string Id => "geom_turbopump";
    public string Name => "Turbopump Geometry";

    public Voxels Run(ThermoTaskInput input)
    {
        return Geometry_Turbopump.Create(input.Params);
    }

    public Voxels Execute(ThermoTaskInput input)
    {
        return Run(input);
    }
}
