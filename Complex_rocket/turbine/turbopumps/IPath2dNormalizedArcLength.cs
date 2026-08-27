// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a 2D path parameterized by normalized arc length.</summary>
/// <remarks>
/// For parameters 0 ≤ t0 ≤ t1 ≤ 1, the arc length traversed between them equals
/// (t1 - t0) * <see cref="IPath2d.fLength"/> within the implementation's numerical accuracy.
/// </remarks>
public interface IPath2dNormalizedArcLength : IPath2d
{
}
