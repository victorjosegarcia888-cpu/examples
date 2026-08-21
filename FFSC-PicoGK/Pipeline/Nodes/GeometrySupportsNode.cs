// GeometrySupportsNode.cs
//
// Node wrapper for engine supports geometry.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Supports;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometrySupportsNode : ITask<Unit, Voxels>
{
    public string Id => "geom_supports";
    public string Name => "Engine Supports";

    public Voxels Run(Unit input)
    {
        return Geometry_Supports.Create();
    }

    public Voxels Execute(Unit input)
    {
        return Run(input);
    }
}
