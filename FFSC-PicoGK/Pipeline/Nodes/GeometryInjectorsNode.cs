// GeometryInjectorsNode.cs
//
// Node wrapper for injector plate geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Injectors;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryInjectorsNode : ITask<ThermoTaskInput, Voxels>
{
    public string Id => "geom_injectors";
    public string Name => "Injectors Geometry";

    public Voxels Execute(ThermoTaskInput input)
    {
        return Geometry_Injectors.Create(input.Params);
    }
}
