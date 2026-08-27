// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a right circular cylinder.</summary>
/// <remarks>
/// The frame origin is the center of the bottom face. The cylinder axis follows local positive Z.
/// </remarks>
public interface ICylinder3d : IPrimitive3d
{
    /// <summary>Cylinder radius.</summary>
    float fRadius { get; }

    /// <summary>Distance between the bottom and top planes.</summary>
    float fHeight { get; }
}
