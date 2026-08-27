// SPDX-License-Identifier: Apache-2.0

namespace PicoGK.Geometry;

/// <summary>Describes a capsule formed by a cylinder and two hemispherical ends.</summary>
/// <remarks>
/// The frame origin is the capsule center. The cylindrical section is centered on the origin and
/// follows local Z. Its end planes are at plus and minus half <see cref="fCylinderHeight"/>.
/// </remarks>
public interface ICapsule3d : IPrimitive3d
{
    /// <summary>Radius shared by the cylindrical section and hemispherical ends.</summary>
    float fRadius { get; }

    /// <summary>Length of the cylindrical section, excluding the hemispherical ends.</summary>
    float fCylinderHeight { get; }
}
