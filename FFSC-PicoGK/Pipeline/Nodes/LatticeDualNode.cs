// LatticeDualNode.cs
//
// Node wrapper for dual-layer adaptive lattice generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class LatticeDualNode : ITask<LatticeDualInput, Voxels>
{
    public string Id => "lattice_dual";
    public string Name => "Dual-Layer Lattice";

    public Voxels Execute(LatticeDualInput input)
    {
        return Lattice_DualLayer.Generate(
            input.StressField,
            input.HighThreshold,
            input.LowThreshold,
            input.HighRadius,
            input.LowRadius);
    }
}
