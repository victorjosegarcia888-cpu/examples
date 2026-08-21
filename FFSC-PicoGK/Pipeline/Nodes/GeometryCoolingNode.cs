// GeometryCoolingNode.cs
//
// Node wrapper for regenerative cooling channels geometry.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Cooling;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryCoolingNode : ITask<CoolingGeometryInput, Voxels>
{
    public string Id => "geom_cooling";
    public string Name => "Cooling Channels Geometry";

    public Voxels Run(CoolingGeometryInput input)
    {
        var primary = Geometry_Cooling.Primary(input.Chamber, input.Spike);
        var secondary = Geometry_Cooling.Secondary(input.Chamber, input.Spike);
        return primary + secondary;
    }

    public Voxels Execute(CoolingGeometryInput input)
    {
        return Run(input);
    }
}
