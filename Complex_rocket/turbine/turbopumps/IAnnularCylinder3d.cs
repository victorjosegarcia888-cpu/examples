// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a hollow right circular cylinder.</summary>
/// <remarks>
/// The frame origin is the center of the bottom face. The cylinder axis follows local positive Z.
/// </remarks>
public interface IAnnularCylinder3d : IPrimitive3d
{
    /// <summary>Inner radius of the annular cross-section.</summary>
    float fInnerRadius { get; }

    /// <summary>Outer radius of the annular cross-section.</summary>
    float fOuterRadius { get; }

    /// <summary>Distance between the bottom and top planes.</summary>
    float fHeight { get; }
}
