// ILattice3D.cs
//
// Interface for 3D lattice generation tasks.

using PicoGK;

namespace PipelineCore;

public interface ILattice3D
{
    string Id { get; }
    string Name { get; }
    Voxels Generate(Voxels stressField);
    Voxels Generate(Voxels stressField, double highThreshold, double lowThreshold, double highRadius, double lowRadius);
    void SetParameters(double scale, double intensity);
}
