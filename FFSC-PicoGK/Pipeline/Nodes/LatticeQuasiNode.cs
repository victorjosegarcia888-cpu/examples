// LatticeQuasiNode.cs
//
// Node wrapper for quasicrystal lattice generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Chamber;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class LatticeQuasiNode : ITask<LatticeQuasiInput, Voxels>
{
    public string Id => "lattice_quasi";
    public string Name => "Quasicrystal Lattice";

    public Voxels Execute(LatticeQuasiInput input)
    {
        return Lattice_Quasicrystal.Generate(
            input.StressField,
            input.Scale,
            input.Intensity);
    }
}
