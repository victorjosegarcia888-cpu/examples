// IGeometry3D.cs
//
// Interface for 3D geometry generation tasks.

using PicoGK;

namespace PipelineCore;

public interface IGeometry3D
{
    string Id { get; }
    string Name { get; }
    Voxels CreateVoxels();
    Mesh? CreateMesh();
    bool IsManifold { get; }
}
