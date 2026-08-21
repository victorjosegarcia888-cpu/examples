// GeometryStructuralNode.cs
//
// Node wrapper for structural frame geometry.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Structural;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryStructuralNode : ITask<Unit, Voxels>
{
    public string Id => "geom_structural";
    public string Name => "Structural Frame";

    public Voxels Execute(Unit input)
    {
        return Geometry_Structural.Create();
    }
}
