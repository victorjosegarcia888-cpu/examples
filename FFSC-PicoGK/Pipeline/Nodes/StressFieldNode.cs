// StressFieldNode.cs
//
// Node wrapper for volumetric stress field analysis.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Physics.Stress;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class StressFieldNode : ITask<StressFieldInput, Voxels>
{
    public string Id => "physics_stress";
    public string Name => "Stress Field Analysis";

    public Voxels Run(StressFieldInput input)
    {
        return StressField.Dynamic(input.Chamber, input.Spike, input.Manifold);
    }

    public Voxels Execute(StressFieldInput input)
    {
        return Run(input);
    }
}
