// GeometryChamberNode.cs
//
// Node wrapper for chamber geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryChamberNode : ITask<ThermoTaskInput, Voxels>
{
    public string Id => "geom_chamber";
    public string Name => "Chamber Geometry";

    public Voxels Execute(ThermoTaskInput input)
    {
        return Geometry_Chamber.Create(input.Params);
    }
}
