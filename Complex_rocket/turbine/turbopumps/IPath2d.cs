// SPDX-License-Identifier: Apache-2.0

using System.Numerics;

namespace PicoGK.Geometry;

/// <summary>Describes a finite path with a normalized parameter in two-dimensional space.</summary>
/// <remarks>
/// The parameter domain is [0,1]. This base contract does not require uniform traversal speed;
/// use <see cref="IPath2dNormalizedArcLength"/> when equal parameter intervals must represent
/// equal arc-length intervals.
/// </remarks>
public interface IPath2d
{
    /// <summary>Returns the point at normalized parameter <paramref name="t"/>.</summary>
    /// <param name="t">Path parameter in [0,1].</param>
    Vector2 vecPtAtT(float t);

    /// <summary>Total path length.</summary>
    float fLength { get; }
}
