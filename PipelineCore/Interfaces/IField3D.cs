// IField3D.cs
//
// Interface for 3D field analysis tasks (stress, CFD, thermal).

using PicoGK;

namespace PipelineCore;

public interface IField3D
{
    string Id { get; }
    string Name { get; }
    Voxels Evaluate(Voxels geometry);
    Voxels EvaluateStatic(Voxels geometry);
    Voxels EvaluateDynamic(Voxels geometry);
    double MaxValue { get; }
    double MinValue { get; }
}
