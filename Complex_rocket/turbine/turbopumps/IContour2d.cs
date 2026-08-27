// SPDX-License-Identifier: Apache-2.0

using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Describes a simple, closed, counterclockwise 2D contour.</summary>
/// <remarks>
/// The points at t=0 and t=1 coincide. Traversal is parameterized by normalized arc length.
/// </remarks>
public interface IContour2d : IPath2dNormalizedArcLength
{
    /// <summary>Returns the contour point and outward unit normal at normalized parameter t.</summary>
    /// <remarks>
    /// At a geometric discontinuity, selection of an outward normal is implementation-defined.
    /// </remarks>
    void PtAtT(float t, out Vector2 vecPoint, out Vector2 vecNormal);
}
