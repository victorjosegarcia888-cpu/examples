// GeometryManifoldCH4Node.cs
//
// Node wrapper for CH4 manifold geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Manifolds;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryManifoldCH4Node : ITask<Unit, Voxels>
{
    public string Id => "geom_manifold_ch4";
    public string Name => "CH4 Manifold Geometry";

    public Voxels Execute(Unit input)
    {
        return Geometry_Manifold_CH4.Create();
    }
}
