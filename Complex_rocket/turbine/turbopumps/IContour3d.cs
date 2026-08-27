// SPDX-License-Identifier: Apache-2.0

using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Describes a simple, closed, planar 3D contour.</summary>
/// <remarks>
/// The points at t=0 and t=1 coincide. Traversal is parameterized by normalized arc length.
/// </remarks>
public interface IContour3d : IPath3dNormalizedArcLength
{
    /// <summary>
    /// Returns the contour point and outward unit normal in the contour plane at normalized
    /// parameter t.
    /// </summary>
    /// <remarks>
    /// At a geometric discontinuity, selection of an outward normal is implementation-defined.
    /// </remarks>
    void PtAtT(float t, out Vector3 vecPoint, out Vector3 vecNormal);
}
