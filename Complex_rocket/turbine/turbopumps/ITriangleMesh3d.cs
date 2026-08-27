// SPDX-License-Identifier: Apache-2.0

using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Provides bulk read access to an indexed triangle mesh in three-dimensional space.</summary>
/// <remarks>
/// Vertex and triangle counts and data must remain consistent for the lifetime of the mesh
/// instance. Implementations may retain any internal representation and need not expose their
/// storage directly.
/// </remarks>
public interface ITriangleMesh3d
{
    /// <summary>Number of vertices.</summary>
    int nVertexCount { get; }

    /// <summary>Number of indexed triangles.</summary>
    int nTriangleCount { get; }

    /// <summary>Copies all vertices into the start of the destination span.</summary>
    /// <exception cref="ArgumentException">
    /// The destination is shorter than <see cref="nVertexCount"/>.
    /// </exception>
    void CopyVertices(Span<Vector3> avecDestination);

    /// <summary>Copies all triangles into the start of the destination span.</summary>
    /// <exception cref="ArgumentException">
    /// The destination is shorter than <see cref="nTriangleCount"/>.
    /// </exception>
    void CopyTriangles(Span<Triangle> atriDestination);
}
