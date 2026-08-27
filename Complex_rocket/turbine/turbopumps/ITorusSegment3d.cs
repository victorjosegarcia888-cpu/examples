// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes an angular segment of a torus with a circular tube.</summary>
/// <remarks>
/// The frame origin is the torus center. The segment starts on local positive X and sweeps from
/// local positive X toward local positive Y about local positive Z. This contract specifies the
/// swept torus surface and does not prescribe whether implementations close the segment ends.
/// </remarks>
public interface ITorusSegment3d : IPrimitive3d
{
    /// <summary>Distance from the torus center to the centerline of its tube.</summary>
    float fMajorRadius { get; }

    /// <summary>Radius of the torus tube.</summary>
    float fMinorRadius { get; }

    /// <summary>Angular sweep about local positive Z.</summary>
    Rad rSweep { get; }
}
