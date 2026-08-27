// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a right circular cone.</summary>
/// <remarks>
/// The frame origin is the center of the circular bottom face. The apex lies on local positive Z.
/// </remarks>
public interface ICone3d : IPrimitive3d
{
    /// <summary>Radius of the bottom face.</summary>
    float fRadius { get; }

    /// <summary>Distance from the bottom plane to the apex.</summary>
    float fHeight { get; }
}
