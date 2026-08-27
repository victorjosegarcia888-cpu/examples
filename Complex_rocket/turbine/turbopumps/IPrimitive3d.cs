// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a geometric primitive positioned in three-dimensional space.</summary>
/// <remarks>
/// The frame maps the primitive's local coordinates to world coordinates. Local positive Z is
/// the primary axis or up direction, and local positive X defines zero azimuth.
/// </remarks>
public interface IPrimitive3d
{
    /// <summary>Primitive-local frame expressed in world coordinates.</summary>
    Frame3d frm { get; }
}
