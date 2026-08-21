// GeometryManifoldLOXNode.cs
//
// Node wrapper for LOX manifold geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryManifoldLOXNode : ITask<Unit, Voxels>
{
    public string Id => "geom_manifold_lox";
    public string Name => "LOX Manifold Geometry";

    public Voxels Run(Unit input)
    {
        return Geometry_Manifold_LOX.Create();
    }

    public Voxels Execute(Unit input)
    {
        return Run(input);
    }
}
