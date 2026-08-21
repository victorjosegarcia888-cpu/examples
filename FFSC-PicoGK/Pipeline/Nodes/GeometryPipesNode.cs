// GeometryPipesNode.cs
//
// Node wrapper for feed system pipes geometry.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Pipes;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryPipesNode : ITask<Unit, Voxels>
{
    public string Id => "geom_pipes";
    public string Name => "Feed System Pipes";

    public Voxels Run(Unit input)
    {
        return Geometry_Pipes.Create();
    }

    public Voxels Execute(Unit input)
    {
        return Run(input);
    }
}
