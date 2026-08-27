// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a sphere.</summary>
/// <remarks>
/// The frame origin is the sphere center. Local positive Z is the polar axis, and local positive X
/// defines zero azimuth.
/// </remarks>
public interface ISphere3d : IPrimitive3d
{
    /// <summary>Sphere radius.</summary>
    float fRadius { get; }
}
