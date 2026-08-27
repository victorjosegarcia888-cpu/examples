// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a torus with a circular tube.</summary>
/// <remarks>
/// The frame origin is the torus center. The symmetry axis follows local positive Z, and the major
/// circle lies in the local XY plane.
/// </remarks>
public interface ITorus3d : IPrimitive3d
{
    /// <summary>Distance from the torus center to the centerline of its tube.</summary>
    float fMajorRadius { get; }

    /// <summary>Radius of the torus tube.</summary>
    float fMinorRadius { get; }
}
