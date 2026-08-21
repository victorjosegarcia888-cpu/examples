// GeometryManifoldFFSCNode.cs
//
// Node wrapper for FFSC manifold geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryManifoldFFSCNode : ITask<Unit, Voxels>
{
    public string Id => "geom_manifold_ffsc";
    public string Name => "FFSC Manifold Geometry";

    public Voxels Execute(Unit input)
    {
        return Geometry_Manifold_FFSC.Create();
    }
}
