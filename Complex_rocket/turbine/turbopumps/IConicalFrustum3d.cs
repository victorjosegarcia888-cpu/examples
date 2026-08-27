// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a right circular conical frustum.</summary>
/// <remarks>
/// The frame origin is the center of the bottom face. The top face is displaced along local
/// positive Z.
/// </remarks>
public interface IConicalFrustum3d : IPrimitive3d
{
    /// <summary>Radius of the bottom face.</summary>
    float fBottomRadius { get; }

    /// <summary>Radius of the top face.</summary>
    float fTopRadius { get; }

    /// <summary>Distance between the bottom and top planes.</summary>
    float fHeight { get; }
}
